using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Store.Models;

namespace Store.Repository
{

    public class StoreRepository: IStoreRepository
    {
        private readonly dbstoreContext _context;
        //private GetConfiguration _configuration;
        public string connectionString = "Server=DESKTOP-NQE73DV;Database=db.store;User Id=sa;Password=123456;Trusted_Connection=True;MultipleActiveResultSets=true";
        public StoreRepository()
        {
            _context = new dbstoreContext();
        }

        public StoreRepository(dbstoreContext context)
        {
            _context = context;
        }
        public List<User> GetAll()
        {
            return _context.User.ToList();
        }
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var storeProduced = "sp_User";

            if (userLogin == null)
            {
                return new ObjectResult(new Response { status = 400, message = "error" }) { StatusCode = 400 };
            }

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var param = new DynamicParameters();
                param.Add("@username", userLogin.Username);
                param.Add("@password", userLogin.Password);
                var result = conn.Query(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure).ToList();
                if(result.Count > 0)
                {
                    return new ObjectResult(new Response { status = 200, message = "success" }) { StatusCode = 200 };
                } else
                {
                    return new ObjectResult(new Response { status = 400, message = "error" }) { StatusCode = 400 };
                }
            }
        }
    }
}
