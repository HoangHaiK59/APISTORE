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
        BaseResponseWithToken Authorize(string token , [FromBody] UserLogin userLogin);
        BaseResponse Subscribe([FromBody] Subscribe subscribe);
        Response<Landing> GetLandingPage(int ordinal);
        Response<Product> GetHomePage();
        Response<Product> GetJacketPage();
        Response<Product> GetShirtPage();
        Response<Product> GetTShirtPage();
        Response<Product> GetJeanPage();
        Response<Product> GetJumpSuitPage();
        Response<Product> GetShortPage();
        Response<Product> GetPrincessPage();
        Response<Product> GetSkirtPage();
        Response<Product> GetDetailProduct(int productId);
        Response<Product> GetHotProduct();

        Response<List<Category>> GetCategoryPage();
        Task<BaseResponse> AddtoCheckout(Product product);
    }
}
