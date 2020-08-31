using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Models;
using Store.Repositories.Interfaces;
using Store.Services.Interfaces;

namespace Store.Services
{
    public class StoreService: IStoreService
    {
        private readonly IStoreRepository _storeRepo;
        private IConfiguration _configuration;
        public StoreService(IStoreRepository storeRepo)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
            _storeRepo = storeRepo;
        }
        public Response<UserInfo> GetUserInfo(Guid userId)
        {
            return _storeRepo.GetUserInfo(userId);
        }
        public BaseResponseWithToken Authorize([FromBody] UserLogin userLogin)
        {
            string access_token = generateJwtToken(userLogin);
            var tokengen = generateRefreshToken();
            tokengen.access_token = access_token;

            return _storeRepo.Authorize(tokengen, userLogin);
        }
        public BaseResponse Subscribe([FromBody] Subscribe subscribe)
        {
            return _storeRepo.Subscribe(subscribe);
        }
        public Response<Landing> GetLandingPage(int ordinal)
        {
            return _storeRepo.GetLandingPage(ordinal);
        }
        public Response<Product> GetHomePage()
        {
            return _storeRepo.GetHomePage();
        }
        public Response<List<Product>> GetProductByCategory(Guid catId, int offSet, int pageSize)
        {
            return _storeRepo.GetProductByCategory(catId, offSet, pageSize);
        }

        public Response<Product> GetDetailProduct(int productId)
        {
            return _storeRepo.GetDetailProduct(productId);
        }
        public Response<Product> GetHotProduct()
        {
            return _storeRepo.GetHotProduct();
        }
        public Response<List<Category>> GetCategoryList()
        {
            return _storeRepo.GetCategoryList();
        }
        public Response<List<ParentCategory>> GetCategoryParentList()
        {
            return _storeRepo.GetCategoryParentList();
        }
        public Task<BaseResponse> AddProduct([FromBody] ProductInfo productInfo)
        {
            return _storeRepo.AddProduct(productInfo);
        }
        public Response<List<Color>> GetColorList()
        {
            return _storeRepo.GetColorList();
        }
        public Response<List<Size>> GetSizeList()
        {
            return _storeRepo.GetSizeList();
        }
        public ProductGet GetAllProduct(int offSet, int pageSize)
        {
            return _storeRepo.GetAllProduct(offSet, pageSize);
        }
        public Response<List<Menu>> GetClientMenu(Guid userId)
        {
            return _storeRepo.GetClientMenu(userId);
        }
        public Response<List<Menu>> GetClientMenuDefault()
        {
            return _storeRepo.GetClientMenuDefault();
        }
        public BaseResponse DeleteProduct(int id)
        {
            return _storeRepo.DeleteProduct(id);
        }
        public BaseResponse Register([FromBody] Register user)
        {
            return _storeRepo.Register(user);
        }
        public Task<BaseResponse> AddToCart([FromBody] Cart product)
        {
            return _storeRepo.AddToCart(product);
        }

        private string generateJwtToken(UserLogin user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Secret").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                          null,
                          null,
                          claims,
                          expires: DateTime.Now.AddDays(1),
                          signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Token generateRefreshToken()
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new Token
                {
                    refresh_token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(1),
                    Created = DateTime.UtcNow,
                };
            }
        }



    }
}
