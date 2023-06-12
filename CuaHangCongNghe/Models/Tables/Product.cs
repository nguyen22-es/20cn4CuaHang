using System;
using System.Collections.Generic;

namespace CuaHangCongNghe.Models.Tables
{
    public partial class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        public int? CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public int? Stockquantity { get; set; }
    }
}
