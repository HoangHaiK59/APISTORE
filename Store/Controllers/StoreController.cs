using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Store.Repository;
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

namespace Store.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private IStoreRepository _storeRepository;
        private IConfiguration _configuration;

        public StoreController(StoreRepository storeRepository)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
            _storeRepository = storeRepository;
        }

        /// <summary>
        ///  Get User Info
        /// </summary>
        /// <param username="string"></param>
        /// <returns></returns>
        [HttpGet("user")]
        public IActionResult GetUserInfo(string username)
        {
            var validate = GetAuthorizeHeader();
            if (validate == null)
            {
                return Unauthorized();
            }
            var model = _storeRepository.GetUserInfo(username);
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
            string token = generateJwtToken(userLogin);
            var result = _storeRepository.Authorize(token , userLogin);
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
            var valid = isValidEmail(subscribe.email);
            if (valid)
            {
                var result = _storeRepository.Subscribe(subscribe);
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Get Landing Page
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetLandingPage")]
        public IActionResult GetLandingPage(int ordinal)
        {
            var result = _storeRepository.GetLandingPage(ordinal);
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
            var result = _storeRepository.GetHomePage();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Get Jacket Page
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetJacketPage")]
        public IActionResult GetJacketPage()
        {
            var result = _storeRepository.GetJacketPage();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Get Jean Page
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetJeanPage")]
        public IActionResult GetJeanPage()
        {
            var result = _storeRepository.GetJeanPage();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Get JumpSuit Page
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetJumpSuitPage")]
        public IActionResult GetJumpSuitPage()
        {
            var result = _storeRepository.GetJumpSuitPage();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Get Princess Page
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDressPage")]
        public IActionResult GetDressPage(int category_id, int offSet)
        {
            var result = _storeRepository.GetDressPage(category_id, offSet);
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Get Shirt Page
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetShirtPage")]
        public IActionResult GetShirtPage()
        {
            var result = _storeRepository.GetShirtPage();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Get TShirt Page
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTShirtPage")]
        public IActionResult GetTShirtPage()
        {
            var result = _storeRepository.GetTShirtPage();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Get Skirt Page
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSkirtPage")]
        public IActionResult GetSkirtPage()
        {
            var result = _storeRepository.GetSkirtPage();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        /// Get Short Page
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetShortPage")]
        public IActionResult GetShortPage()
        {
            var result = _storeRepository.GetShortPage();
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
            var result = _storeRepository.GetCategoryList();
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
            var result = _storeRepository.GetCategoryParentList();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Add product
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductInfo productInfo)
        {
            var validate = GetAuthorizeHeader();
            if (validate == null)
            {
                return Unauthorized();
            }
            var result = await _storeRepository.AddProduct(productInfo);
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
            var result = _storeRepository.GetColorList();
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
            var result = _storeRepository.GetSizeList();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Get Detail Product
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDetailProduct")]
        public IActionResult GetDetailProduct(int id)
        {
            var result = _storeRepository.GetDetailProduct(id);
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  Get Hot Product
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHotProduct")]
        public IActionResult GetHotProduct()
        {
            var result = _storeRepository.GetHotProduct();
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest();
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
            if(string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                string DomainMapper( Match match )
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

        JwtSecurityToken GetAuthorizeHeader()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var validate = ValidateToken(token);
            return validate;
        }

    }
}
