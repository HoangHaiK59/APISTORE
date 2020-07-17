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

        }
        public List<User> GetAll()
        {
            return _context.User.ToList();
        }
        public BaseResponseWithToken Token([FromBody] UserLogin userLogin)
        {
            var storeProduced = "sp_User";

            using (var conn = new SqlConnection(_configuration.GetConnectionString("DBStore")))
            {
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("@username", userLogin.Username);
                    param.Add("@password", userLogin.Password);
                    var result = conn.QueryFirst<BaseResponseWithToken>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure);
                    var token = generateJwtToken(userLogin);
                    if(result.status)
                    {
                        result.token = token;
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
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                          null,
                          expires: DateTime.Now.AddDays(1),
                          signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
