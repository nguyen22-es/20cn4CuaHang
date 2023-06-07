

namespace CuaHangCongNghe.Controllers.laydulieu
{
    public partial class thongtinsanpham
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int OrderItemsId { get; set; }
        public string Name { get; set; }

        public int Quantity { get; set; }
        public DateOnly OrderDate { get; set; }
        public string? Status { get; set; }
           
        public int ProductId { get; set; }
       
        public float Price { get; set; }
        public int Id { get; set; }
   
        public string? Description { get; set; } 




    }
}
