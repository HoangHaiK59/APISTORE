using System;
using System.Collections.Generic;

namespace Store.Models
{
    public partial class Marketing
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Ordinal { get; set; }
    }
}
