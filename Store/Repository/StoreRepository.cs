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
using Store.Models;

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
        public async Task<Response> Login([FromBody] UserLogin userLogin)
        {
            var storeProduced = "sp_User";

            using (var conn = new SqlConnection(_configuration.GetConnectionString("DBStore")))
            {
                conn.Open();
                var param = new DynamicParameters();
                param.Add("@username", userLogin.Username);
                param.Add("@password", userLogin.Password);
                var result = await conn.QueryFirstAsync<Response>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
