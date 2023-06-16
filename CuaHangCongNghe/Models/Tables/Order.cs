using System;
using System.Collections.Generic;

namespace CuaHangCongNghe.Models.Tables
{
    public partial class Order
    {
        public Order()
        {
            Orderitems = new HashSet<Orderitem>();
        }

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Status { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Orderitem> Orderitems { get; set; }
    }
}
