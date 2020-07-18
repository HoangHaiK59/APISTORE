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
        Response<UserInfo> GetUserInfo(string username);
        BaseResponseWithToken Token([FromBody] UserLogin userLogin);
    }
}
