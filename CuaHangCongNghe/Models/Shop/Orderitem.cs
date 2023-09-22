using System;
using System.Collections.Generic;

namespace CuaHangCongNghe.Models.Shop
{
    public partial class Orderitem
    {
        public int OrderItemsId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
