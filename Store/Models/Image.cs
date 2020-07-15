using System;
using System.Collections.Generic;

namespace Store.Models
{
    public partial class Image
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Color { get; set; }
    }
}
