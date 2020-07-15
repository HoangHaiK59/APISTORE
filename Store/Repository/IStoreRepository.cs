using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store.Models;

namespace Store.Repository
{
    interface IStoreRepository
    {
        List<User> GetAll();
    }
}
