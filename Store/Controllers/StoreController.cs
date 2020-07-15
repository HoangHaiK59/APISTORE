using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Store.Repository;
using Store.Models;

namespace Store.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet]
        public List<User> Index()
        {
            var model = _storeRepository.GetAll();
            //return View(model);
            return model;
        }
    }
}
