using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Store.Models;

namespace Store.Services.Interfaces
{
    public interface IStoreService
    {
        Response<UserInfo> GetUserInfo(Guid userId);
        BaseResponseWithToken Authorize( [FromBody] UserLogin userLogin);
        BaseResponse Subscribe([FromBody] Subscribe subscribe);
        Response<Landing> GetLandingPage(int ordinal);
        Response<Product> GetHomePage();
        Response<List<Product>> GetProductByCategory(int category_id, int offSet, int pageSize);
        Response<Product> GetDetailProduct(int productId);
        Response<Product> GetHotProduct();

        Response<List<Category>> GetCategoryList();

        Response<List<ParentCategory>> GetCategoryParentList();
        Task<BaseResponse> AddProduct([FromBody] ProductInfo productInfo);
        Response<List<Color>> GetColorList();
        Response<List<Size>> GetSizeList();
        ProductGet GetAllProduct(int offSet, int pageSize);
        Response<List<Menu>> GetClientMenu();
        Response<List<Menu>> GetClientMenuDefault();
        BaseResponse DeleteProduct(int id);
        BaseResponse Register([FromBody] Register user);
        Task<BaseResponse> AddtoCheckout([FromBody] Product product);
    }
}
