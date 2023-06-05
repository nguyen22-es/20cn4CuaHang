using System;
using System.Collections.Generic;

namespace CuaHangCongNghe.Models.Tables
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int UserId { get; set; }
        public string? NameUser { get; set; }
        public string? Password { get; set; }
        public string? EmailUser { get; set; }
        public string? AddressUser { get; set; }
        public string PhoneUser { get; set; } = null!;
        public DateOnly RegistrationDate { get; set; }
        public string Role { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
