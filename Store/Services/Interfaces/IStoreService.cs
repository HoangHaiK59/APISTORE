﻿using System;
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
        Response<List<Product>> GetProductByCategory(string catId, int offSet, int pageSize);
        Response<Product> GetDetailProduct(int productId);
        Response<Product> GetHotProduct();

        Response<List<Category>> GetCategoryList();

        Response<List<ParentCategory>> GetCategoryParentList();
        Task<BaseResponse> AddProduct([FromBody] ProductInfo productInfo);
        Response<List<Color>> GetColorList();
        Response<List<Size>> GetSizeList();
        Response<ProductGet> GetAllProduct(int offSet, int pageSize);
        Response<List<Product>> GetProducts(int offSet, int pageSize);
        Response<List<Menu>> GetClientMenu(Guid userId);
        Response<List<Menu>> GetClientMenuDefault();
        BaseResponse DeleteProduct(int id);
        BaseResponse Register([FromBody] Register user);
        Task<BaseResponse> AddToCart([FromBody] Cart product);
    }
}
