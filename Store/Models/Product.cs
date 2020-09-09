using System;
using System.Collections.Generic;

namespace Store.Models
{
    public partial class Product
    {
        public long Id { get; set; }
        public Guid CatId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long Discount { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public long View { get; set; }
        public string size { get; set; }
        public List<Image> images { get; set; }
        public string image { get; set; }
        public bool status { get; set; }
        public string statusName { get; set; }
    }

    public partial class Cart
    {
        public long Id { get; set; }
        public Guid CatId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long Discount { get; set; }
        public string size { get; set; }
    }

    public partial class ProductGet
    {
        public long total { get; set; }
        public List<Product> products { get; set; }
    }

    public class ColorMapUrl
    {
        public string color { get; set; }
        public string url { get; set; }
    }

    public class ProductInfo
    {
        public ProductSet product { get; set; }
        public List<ColorMapUrl> images { get; set; }
    }

    public partial class ProductSet
    {
        public long Id { get; set; }
        public Guid catId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long Discount { get; set; }
        public string Description { get; set; }
        public string[] size { get; set; }
        public string image { get; set; }
        public bool status { get; set; }
    }
}
