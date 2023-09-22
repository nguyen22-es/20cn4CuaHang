using System;

namespace CuaHangCongNghe.Models
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public ProductViewModel Product { get; set; }
        public int quantity { get; set; }

        public double Price => quantity * Product.Price;

    }
}
