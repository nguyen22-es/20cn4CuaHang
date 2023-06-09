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
        public string? EmailUser { get; set; }
        public string? AddressUser { get; set; }
        public string? PhoneUser { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int? Idrole { get; set; }
        public int? Iddangnhap { get; set; }

        public virtual Dangnhapuser? IddangnhapNavigation { get; set; }
        public virtual Namerole? IdroleNavigation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
