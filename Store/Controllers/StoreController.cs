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

        [HttpGet("User")]
        public List<User> Index()
        {
            var model = _storeRepository.GetAll();
            //return View(model);
            return model;
        }

        [HttpPost("Login")]
        public BaseResponseWithToken Login([FromBody] UserLogin userLogin)
        {
            var result = _storeRepository.Login(userLogin);
            return result;
        }
    }
}
