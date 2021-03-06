﻿using System;
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
using Newtonsoft.Json;
using Store.Repositories.Interfaces;

namespace Store.Repositories
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
        public Response<UserInfo> GetUserInfo( Guid userId)
        {
            var storeProduced = "sp_User_Get";
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = new Response<UserInfo>();
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("@userId", userId);
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

        public BaseResponseWithToken Authorize(Token token, [FromBody] UserLogin userLogin)
        {
            var storeProduced = "sp_User";

            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("@email", userLogin.Email);
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
        public Response<List<Product>> GetProductByCategory(string cat_id, int offSet, int pageSize)
        {
            var storeProduced = "sp_Product_Get";
            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("cat_id", cat_id);
                    param.Add("offSet", offSet);
                    param.Add("pageSize", pageSize);
                    var result = conn.QueryMultiple(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure);
                    if (result != null)
                    {
                        var infoList = result.Read<Product>().ToList();

                        var images = result.Read<Image>().ToList();
                        foreach(var dt in infoList)
                        {
                            dt.images = images.Where(i => i.ProductId == dt.Id).ToList();
                        }
                        return new Response<List<Product>>{ success = true, data = infoList  };
                    }
                    return new Response<List<Product>> { success = true, data = new List<Product>() };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
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
            var storeProduced = "sp_Product_Detail";

            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("@id", productId);
                    var reader = conn.QueryMultiple(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure);
                    if (reader != null)
                    {
                        var info = reader.Read<Product>().ToList();
                        var images = reader.Read<Image>().ToList();
                        foreach (var dt in info)
                        {
                            dt.images = images;
                        }
                        return new Response<Product> { success = true, data = info[0] };
                    }
                    return new Response<Product> { success = true, data = new Product() };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public BaseResponse DeleteProduct(int id)
        {
            var storeProduced = "sp_Product_Delete";
            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("@id", id);
                    var result = conn.QueryFirst<BaseResponse>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public BaseResponse Register([FromBody] Register user)
        {
            var storeProduced = "sp_User_Register";
            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("@password", user.Password);
                    param.Add("@firstname", user.FirstName);
                    param.Add("@lastname", user.LastName);
                    param.Add("@email", user.Email);
                    param.Add("@phone", user.Phone);
                    param.Add("@address", user.Address);
                    var result = conn.QueryFirst<BaseResponse>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Response<ProductGet> GetAllProduct(int offSet, int pageSize)
        {
            var storeProduced = "sp_Product";

            using (var conn = new SqlConnection(_connectionString))
            {
                var result = new Response<ProductGet>();
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("@offSet", offSet);
                    param.Add("@pageSize", pageSize);
                    var reader = conn.QueryMultiple(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure);
                    if(reader != null)
                    {
                        var total = reader.Read<long>().First();
                        var products = reader.Read<Product>().ToList();
                        var images = reader.Read<Image>().ToList();
                        foreach (var dt in products)
                        {
                            dt.images = images.Where(i => i.ProductId == dt.Id).ToList();
                        }
                        return new Response<ProductGet> { success = true, data = new ProductGet() { total = total, products = products } };
                    }
                    return new Response<ProductGet> { success = true ,data = { total = 0, products = new List<Product>() } };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public Response<List<Product>> GetProducts(int offSet, int pageSize)
        {
            var storeProduced = "sp_Products_Page";

            using (var conn = new SqlConnection(_connectionString))
            {
                var result = new Response<ProductGet>();
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("@offSet", offSet);
                    param.Add("@pageSize", pageSize);
                    var reader = conn.QueryMultiple(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure);
                    if (reader != null)
                    {
                        var products = reader.Read<Product>().ToList();
                        var images = reader.Read<Image>().ToList();
                        foreach (var dt in products)
                        {
                            dt.images = images.Where(i => i.ProductId == dt.Id).ToList();
                        }
                        return new Response<List<Product>> { success = true, data= products };
                    }
                    return new Response<List<Product>> { success = true, data = new List<Product>() };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public Response<List<Menu>> GetClientMenu(Guid userId)
        {
            var storeProduced = "sp_Menu";
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = new Response<List<Menu>>();
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("@userId", userId);
                    var data = conn.Query<Menu>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    if(data.Count > 0)
                    {
                        result.data = data;
                        result.success = true;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public Response<List<Menu>> GetClientMenuDefault()
        {
            var storeProduced = "sp_Menu_Default";
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = new Response<List<Menu>>();
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    var data = conn.Query<Menu>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    if (data.Count > 0)
                    {
                        result.data = data;
                        result.success = true;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public async Task<BaseResponse> AddToCart([FromBody] Cart product)
        {
            var storeProduced = "sp_Cart_Set";

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

        public async Task<BaseResponse> AddProduct([FromBody] ProductInfo productInfo)
        {
            var storeProduced = "sp_Product_Set";
            // string[] converts = (from i in productInfo.images select JsonConvert.SerializeObject(i)).To
            string images = JsonConvert.SerializeObject(productInfo.images);
            string[] sizeConvert = (from i in productInfo.product.size select i.ToString()).ToArray<string>();
            string size = string.Join(",", sizeConvert);
            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    var param = new DynamicParameters();
                    param.Add("@id", productInfo.product.Id);
                    param.Add("@catId", productInfo.product.catId);
                    param.Add("@name", productInfo.product.Name);
                    param.Add("@price", productInfo.product.Price);
                    param.Add("@discount", productInfo.product.Discount);
                    param.Add("@description", productInfo.product.Description);
                    param.Add("@image", productInfo.product.image);
                    param.Add("@status", productInfo.product.status);
                    param.Add("@size", size);
                    param.Add("@images", images);
                    var result = await conn.QueryFirstAsync<BaseResponse>(storeProduced, param, commandType: System.Data.CommandType.StoredProcedure);
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
