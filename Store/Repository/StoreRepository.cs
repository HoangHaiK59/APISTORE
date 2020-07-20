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

        public BaseResponseWithToken Authorize(string token , [FromBody] UserLogin userLogin)
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
        public BaseResponse Subscribe([FromBody] Subscribe subscribe)
        {
            var storeProduced = "sp_Subscribe_Set";

            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("@email", subscribe.email);
                    var result = conn.QueryFirst<BaseResponse>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

    }
}
