using System;
using System.Collections.Generic;

namespace CuaHangCongNghe.Models.son1
{
    public partial class Order
    {
        public Order()
        {
            Orderitems = new HashSet<Orderitem>();
        }

        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }

        public virtual Status? StatusNavigation { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Orderitem> Orderitems { get; set; }
    }
}
