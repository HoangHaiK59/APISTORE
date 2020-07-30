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
        Response<List<Product>> GetDressPage(int category_id, int offSet);
        Response<Product> GetSkirtPage();
        Response<Product> GetDetailProduct(int productId);
        Response<Product> GetHotProduct();

        Response<List<Category>> GetCategoryList();

        Response<List<ParentCategory>> GetCategoryParentList();
        Task<BaseResponse> AddProduct([FromBody] ProductInfo productInfo);
        Response<List<Color>> GetColorList();
        Response<List<Size>> GetSizeList();
        Task<BaseResponse> AddtoCheckout(Product product);
    }
}
