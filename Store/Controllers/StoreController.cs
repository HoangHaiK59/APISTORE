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

namespace Store.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private IStoreRepository _storeRepository;

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public StoreController(StoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        [HttpGet("user")]
        public IActionResult GetUserInfo(string username)
        {
            var response = Unauthorized();
            var model = _storeRepository.GetUserInfo(username);
            return Ok(model);
        }

        [HttpPost("token")]
        public BaseResponseWithToken Token([FromBody] UserLogin userLogin)
        {
            var result = _storeRepository.Token(userLogin);
            return result;
        }
    }
}
