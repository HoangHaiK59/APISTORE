using System;
using System.Collections.Generic;

namespace Store.Models
{
    public partial class Product
    {
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long Discount { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public long View { get; set; }
        public string size { get; set; }
        public string image_url { get; set; }
    }

    public class ColorMapUrl
    {
        public string color { get; set; }
        public string url { get; set; }
    }

    public class ProductInfo
    {
        public ProductSet product { get; set; }
        public ColorMapUrl[] image_url { get; set; }
    }

    public partial class ProductSet
    {
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long Discount { get; set; }
        public string Description { get; set; }
        public string[] size { get; set; }
    }
}
