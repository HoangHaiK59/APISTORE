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
                        result.success = true;
                    } else
                    {
                        result.success = false;
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
                    if(result.success)
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

        public Response<Landing> GetLandingPage(int ordinal)
        {
            var storeProduced = "sp_Landing_Get";

            using (var conn = new SqlConnection(_connectionString))
            {
                var result = new Response<Landing>();
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("@ordinal", ordinal);
                    var data = conn.QueryFirst<Landing>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure);
                    if(data != null)
                    {
                        result.data = data;
                        result.success = true;
                        return result;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Response<Product> GetHomePage()
        {
            var result = new Response<Product>();
            return result;
        }

        public Response<Product> GetJacketPage()
        {
            var result = new Response<Product>();
            return result;
        }
        public Response<Product> GetShirtPage()
        {
            var result = new Response<Product>();
            return result;
        }
        public Response<Product> GetTShirtPage()
        {
            var result = new Response<Product>();
            return result;
        }
        public Response<Product> GetJeanPage()
        {
            var result = new Response<Product>();
            return result;
        }
        public Response<Product> GetJumpSuitPage()
        {
            var result = new Response<Product>();
            return result;
        }
        public Response<Product> GetShortPage()
        {
            var result = new Response<Product>();
            return result;
        }
        public Response<Product> GetPrincessPage()
        {
            var result = new Response<Product>();
            return result;
        }
        public Response<Product> GetSkirtPage()
        {
            var result = new Response<Product>();
            return result;
        }
        public Response<List<Category>> GetCategoryList() { 
        

            var storeProduced = "sp_Category";
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = new Response<List<Category>>();
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    var data = conn.Query<Category>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    if (data != null)
                    {
                        result.data = data;
                        result.success = true;
                        return result;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Response<List<Color>> GetColorList()
        {
            var storeProduced = "sp_Color_Get";
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = new Response<List<Color>>();
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    var data = conn.Query<Color>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    if (data != null)
                    {
                        result.data = data;
                        result.success = true;
                        return result;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Response<List<Size>> GetSizeList()
        {
            var storeProduced = "sp_Size";
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = new Response<List<Size>>();
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    var data = conn.Query<Size>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    if (data != null)
                    {
                        result.data = data;
                        result.success = true;
                        return result;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public Response<List<ParentCategory>> GetCategoryParentList()
        {
            var storeProduced = "sp_Parent_Category";
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = new Response<List<ParentCategory>>();
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    var data = conn.Query<ParentCategory>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    if (data != null)
                    {
                        result.data = data;
                        result.success = true;
                        return result;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public Response<Product> GetDetailProduct(int productId)
        {
            var result = new Response<Product>();
            return result;
        }

        public async Task<BaseResponse> AddtoCheckout(Product product)
        {
            var storeProduced = "sp_Checkout_Set";

            using (var conn = new SqlConnection(_connectionString))
            {
                var result = new BaseResponse();
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("@product", product);
                    var data = await conn.QueryFirstAsync<BaseResponse>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure);
                    if (data != null)
                    {
                        result.success = true;
                        return result;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<BaseResponse> AddProduct(ProductSet product)
        {
            var storeProduced = "sp_Product_Set";
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = new BaseResponse();
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("@id", product.Id);
                    param.Add("@category_id", product.CategoryId);
                    param.Add("@name", product.Name);
                    param.Add("@price", product.Price);
                    param.Add("@discount", product.Discount);
                    param.Add("@description", product.Description);
                    var data = await conn.QueryAsync<BaseResponse>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure);
                    if (data != null)
                    {
                        result.success = true;
                        return result;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Response<Product> GetHotProduct()
        {
            var result = new Response<Product>();
            return result;
        }

    }
}
