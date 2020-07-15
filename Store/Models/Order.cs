using System;
using System.Collections.Generic;

namespace Store.Models
{
    public partial class Order
    {
        public long Id { get; set; }
        public long TranId { get; set; }
        public long ProductId { get; set; }
        public int Qty { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
    }
}
