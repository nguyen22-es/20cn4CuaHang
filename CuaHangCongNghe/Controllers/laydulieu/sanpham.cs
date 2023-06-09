namespace CuaHangCongNghe.Controllers.laydulieu
{
    public class sanpham
    {
        public int Idcategory { get; set; }
        public string? Namecategory { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        public int? CategoryId { get; set; }
        public string ImageUrl { get; set; } 
        public int Stockquantity { get; set; }
    }
}
