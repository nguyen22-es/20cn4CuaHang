
using System;

namespace CuaHangCongNghe.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string? Namecategory { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public IFormFile File { get; set; }
        public string ImageUrl { get; set; }
        public int Stockquantity { get; set; }
    }
}