using System;
using System.Collections.Generic;

namespace CuaHangCongNghe.Models.son1
{
    public partial class Dangnhapuser
    {
        public Dangnhapuser()
        {
            Users = new HashSet<User>();
        }

        public int Iddangnhap { get; set; }
        public string Password { get; set; }
        public string Tendangnhap { get; set; }
        public int Idrole { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
