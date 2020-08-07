using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Store.Models;

namespace Store.Repositories.Interfaces
{
    public interface IStoreRepository
    {
        Response<UserInfo> GetUserInfo(Guid userId);
        BaseResponseWithToken Authorize(Token token, [FromBody] UserLogin userLogin);
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
        BaseResponse DeleteProduct(int id);
        Response<List<Menu>> GetClientMenu(Guid userId);
        Response<List<Menu>> GetClientMenuDefault();
        BaseResponse Register([FromBody] Register user);
        Task<BaseResponse> AddtoCheckout([FromBody] Product product);
    }
}
