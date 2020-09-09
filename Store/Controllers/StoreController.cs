using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Store.Models;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Store.Services.Interfaces;

namespace Store.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly IStoreService _storeService;
        public StoreController(IStoreService storeService)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
            _storeService = storeService;
        }

        /// <summary>
        ///  Get User Info
        /// </summary>
        /// <param email="string"></param>
        /// <returns></returns>
        [HttpGet("user")]
        public IActionResult GetUserInfo(Guid userId)
        {
            var validate = GetAuthorizeHeader();
            if (validate == null)
            {
                return Unauthorized();
            }
            var model = _storeService.GetUserInfo(userId);
            return Ok(model);
        }

        /// <summary>
        ///  Authorize
        /// </summary>
        /// <param userLogin="UserLogin"></param>
        /// <returns></returns>
        [HttpPost("authorize")]
        public BaseResponseWithToken Authorize([FromBody] UserLogin userLogin)
        {
            var result = _storeService.Authorize(userLogin);
            return result;
        }

        /// <summary>
        ///  Subscribe
        /// </summary>
        /// <param email="string"></param>
        /// <returns></returns>
        [HttpPost("subscribe")]
        public IActionResult Subscribe([FromBody] Subscribe subscribe)
        {
           if(isValidEmail(subscribe.email) && !string.IsNullOrEmpty(subscribe.email))
            {
                var result = _storeService.Subscribe(subscribe);
                return Ok(result);
            }
           return BadRequest();
        }

        /// <summary>
        ///  Get Landing Page
        /// </summary>
        /// <param ordinal="int"></param>
        /// <returns></returns>
        [HttpGet("GetLandingPage")]
        public IActionResult GetLandingPage(int ordinal)
        {
            var result = _storeService.GetLandingPage(ordinal);
            if(result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Get Home Page
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHomePage")]
        public IActionResult GetHomePage()
        {
            var result = _storeService.GetHomePage();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        /// Get Product By Category
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProductByCategory")]
        public IActionResult GetProductByCategory(string catId, int offSet, int pageSize)
        {
            var result = _storeService.GetProductByCategory(catId, offSet, pageSize);
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }


        /// <summary>
        /// Get Category List
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCategoryList")]
        public IActionResult GetCategoryList()
        {
            var result = _storeService.GetCategoryList();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        /// Get Category Parent List
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCategoryParentList")]
        public IActionResult GetCategoryParentList()
        {
            var result = _storeService.GetCategoryParentList();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Add product
        /// </summary>
        /// <param productInfo="ProductInfo"></param>
        /// <returns></returns>
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductInfo productInfo)
        {
            var validate = GetAuthorizeHeader();
            if (validate == null)
            {
                return Unauthorized();
            }
            var result = await _storeService.AddProduct(productInfo);
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Get color list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetColorList")]
        public IActionResult GetColorList()
        {
            var result = _storeService.GetColorList();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Get Size list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSizeList")]
        public IActionResult GetSizeList()
        {
            var result = _storeService.GetSizeList();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Get Detail Product
        /// </summary>
        ///  <param id="int"></param>
        /// <returns></returns>
        [HttpGet("GetDetailProduct")]
        public IActionResult GetDetailProduct(int id)
        {
            var result = _storeService.GetDetailProduct(id);
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Get All Product
        /// </summary>
        /// <param offSet="int"></param>
        /// <param pageSize="int"></param>
        /// <returns></returns>
        [HttpGet("GetAllProduct")]
        public IActionResult GetAllProduct(int offSet, int pageSize)
        {
            var validate = GetAuthorizeHeader();
            if(validate == null)
            {
                return Unauthorized();
            }
            var result = _storeService.GetAllProduct(offSet, pageSize);
            return Ok(result);
        }

        /// <summary>
        ///  Get Products Page
        /// </summary>
        /// <param offSet="int"></param>
        /// <param pageSize="int"></param>
        /// <returns></returns>
        [HttpGet("GetProducts")]
        public IActionResult GetProducts(int offSet, int pageSize)
        {
            var result = _storeService.GetProducts(offSet, pageSize);
            return Ok(result);
        }

        /// <summary>
        ///  Get Client Menu
        /// </summary>
        /// <param userId="string"></param>
        /// <returns></returns>
        [HttpGet("GetClientMenu")]
        public IActionResult GetClientMenu(string userId)
        {
            if(string.IsNullOrEmpty(userId))
            {
                var response = _storeService.GetClientMenuDefault();
                if (response.success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest();
                }
            } else
            {
                Guid userIdGuid = new Guid(userId);
                var validate = GetAuthorizeHeader();
                if (validate == null)
                {
                    var response = _storeService.GetClientMenuDefault();
                    if (response.success)
                    {
                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                var result = _storeService.GetClientMenu(userIdGuid);
                if (result.success)
                {
                    return Ok(result);
                }
                return BadRequest();
            }

        }

        /// <summary>
        ///  Get Client Menu Default
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetClientMenuDefault")]
        public IActionResult GetClientMenuDefault()
        {
            var result = _storeService.GetClientMenuDefault();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Delete Product
        /// </summary>
        /// <param id="int"></param>
        /// <returns></returns>
        [HttpDelete("DeleteProduct")]
        public IActionResult DeleteProduct(int id)
        {
            var validate = GetAuthorizeHeader();
            if (validate == null)
            {
                return Unauthorized();
            }
            var result = _storeService.DeleteProduct(id);
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Add to Cart
        /// </summary>
        /// <param product="Cart"></param>
        /// <returns></returns>
        [HttpDelete("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] Cart product)
        {
            var validate = GetAuthorizeHeader();
            if (validate == null)
            {
                return Unauthorized();
            }
            var result = await _storeService.AddToCart(product);
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Register
        /// </summary>
        /// <param user="Register"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public IActionResult Register([FromBody] Register user)
        {
            var result = _storeService.Register(user);
            return Ok(result);
        }

        /// <summary>
        ///  Get Hot Product
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHotProduct")]
        public IActionResult GetHotProduct()
        {
            var result = _storeService.GetHotProduct();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        JwtSecurityToken GetAuthorizeHeader()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return null;
            };
            var validate = ValidateToken(token);
            return validate;
        }

        private JwtSecurityToken ValidateToken(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Secret").Value));
            var validationParameters = new TokenValidationParameters
            {
                // We recommend 5 minutes or less:
                ClockSkew = TimeSpan.FromMinutes(5),
                ValidateIssuer = false,
                ValidateAudience = false,
                // Specify the key used to sign the token:
                IssuerSigningKey = securityKey,
                RequireSignedTokens = true,
                // Ensure the token hasn't expired:
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
            try
            {
                var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out var rawValidatedToken);
                return (JwtSecurityToken)rawValidatedToken;
            }
            catch (SecurityTokenValidationException stve)
            {
                return null;
            }
            catch (ArgumentException argex)
            {
                return null;
            }
        }

        private static bool isValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    var domainName = idn.GetAscii(match.Groups[2].Value);
                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }
            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

    }
}
