using System;
using System.Collections.Generic;

namespace CuaHangCongNghe.Models.Tables
{
    public partial class Namerole
    {
        public Namerole()
        {
            Users = new HashSet<User>();
        }

        public int Idrole { get; set; }
        public string? Tenrole { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
