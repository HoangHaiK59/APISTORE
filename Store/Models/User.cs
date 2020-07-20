using System;
using System.Collections.Generic;

namespace Store.Models
{
    public partial class User
    {
        public long Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long Created { get; set; }
    }

    public class UserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Rememberme { get; set; }
    }

    public partial class UserInfo
    {
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public Guid UserId { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public long Created { get; set; }
    }
}
