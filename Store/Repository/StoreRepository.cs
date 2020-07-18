using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Store.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Store.Repository
{

    public class StoreRepository: IStoreRepository
    {
        private readonly dbstoreContext _context;
        private IConfiguration _configuration;
        private string _connectionString;
        private string _token;
        public StoreRepository()
        {
            _context = new dbstoreContext();
        }

        public StoreRepository(dbstoreContext context)
        {
            _context = context;
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration =  builder.Build();
            _connectionString = _configuration.GetConnectionString("DBStore");

        }
        public Response<UserInfo> GetUserInfo( string username)
        {
            var storeProduced = "sp_User_Get";
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = new Response<UserInfo>();
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("@username", username);
                    var data = conn.QueryFirst<UserInfo>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure);
                    if(data != null)
                    {
                        result.data = data;
                        result.status = true;
                    } else
                    {
                        result.status = false;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public BaseResponseWithToken Token([FromBody] UserLogin userLogin)
        {
            var storeProduced = "sp_User";

            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("@username", userLogin.Username);
                    param.Add("@password", userLogin.Password);
                    var result = conn.QueryFirst<BaseResponseWithToken>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure);
                    _token = generateJwtToken(userLogin);
                    if(result.status)
                    {
                        result.token = _token;
                    }
                    return result;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

        public string generateJwtToken(UserLogin user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Secret").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                          null,
                          expires: DateTime.Now.AddDays(1),
                          signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static JwtSecurityToken ValidateToken(string token, IEnumerable<SecurityKey> signingKeys)
        {
            var validationParameters = new TokenValidationParameters
            {
                // We recommend 5 minutes or less:
                ClockSkew = TimeSpan.FromMinutes(5),
                // Specify the key used to sign the token:
                IssuerSigningKeys = signingKeys,
                RequireSignedTokens = true,
                // Ensure the token hasn't expired:
                RequireExpirationTime = true,
                ValidateLifetime = true,
            };
            try
            {
                var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out var rawValidatedToken);
                return (JwtSecurityToken)rawValidatedToken;
            }
            catch (SecurityTokenValidationException stve)
            {
                throw new Exception($"Token fail validation: {stve.Message}");
            }
            catch (ArgumentException argex)
            {
                throw new Exception($"Token was invalid: {argex.Message}");
            }
        }
    }
}
