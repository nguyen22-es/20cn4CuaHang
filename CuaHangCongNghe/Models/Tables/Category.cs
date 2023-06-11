using System;
using System.Collections.Generic;

namespace CuaHangCongNghe.Models.Tables
{
    public partial class Category
    {
        public Category()
        {
            Orderitems = new HashSet<Orderitem>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Orderitem> Orderitems { get; set; }
    }
}
