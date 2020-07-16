using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Store.Models;

namespace Store.Repository
{
    interface IStoreRepository
    {
        List<User> GetAll();
        IActionResult Login([FromBody] UserLogin userLogin);
    }
}
