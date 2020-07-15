using System;
using System.Collections.Generic;

namespace Store.Models
{
    public partial class Transaction
    {
        public long Id { get; set; }
        public int Status { get; set; }
        public long UserId { get; set; }
        public string CustId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public decimal Amount { get; set; }
        public string Payment { get; set; }
        public string PaymentInfo { get; set; }
        public string Message { get; set; }
        public string Security { get; set; }
        public long Created { get; set; }
    }
}
