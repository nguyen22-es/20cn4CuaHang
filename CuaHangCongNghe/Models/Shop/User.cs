using System;
using System.Collections.Generic;

namespace CuaHangCongNghe.Models.Shop
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public string UserId { get; set; } = null!;
        public string? NameUser { get; set; }
        public string? EmailUser { get; set; }
        public string? AddressUser { get; set; }
        public string? PhoneUser { get; set; }
        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
