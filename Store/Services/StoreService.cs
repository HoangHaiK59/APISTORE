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
        public Response<UserInfo> GetUserInfo(string username)
        {
            return _storeRepo.GetUserInfo(username);
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
        public Response<Product> GetJacketPage()
        {
            return _storeRepo.GetJacketPage();
        }
        public Response<Product> GetShirtPage()
        {
            return _storeRepo.GetShirtPage();
        }
        public Response<Product> GetTShirtPage()
        {
            return _storeRepo.GetTShirtPage();
        }
        public Response<Product> GetJeanPage()
        {
            return _storeRepo.GetJeanPage();
        }
        public Response<Product> GetJumpSuitPage()
        {
            return _storeRepo.GetJumpSuitPage();
        }
        public Response<Product> GetShortPage()
        {
            return _storeRepo.GetShortPage();
        }
        public Response<List<Product>> GetDressPage(int category_id, int offSet)
        {
            return _storeRepo.GetDressPage(category_id, offSet);
        }
        public Response<Product> GetSkirtPage()
        {
            return _storeRepo.GetSkirtPage();
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
        public List<Product> GetAllProduct(int offSet, int pageSize)
        {
            return _storeRepo.GetAllProduct(offSet, pageSize);
        }
        public Task<BaseResponse> AddtoCheckout([FromBody] Product product)
        {
            return _storeRepo.AddtoCheckout(product);
        }

        private string generateJwtToken(UserLogin user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Secret").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
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
