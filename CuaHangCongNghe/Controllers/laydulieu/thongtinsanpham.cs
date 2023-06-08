

using CuaHangCongNghe.Models.Tables;
using Microsoft.AspNetCore.Mvc;

namespace CuaHangCongNghe.Controllers
{
    public class thongtinsanpham : Controller
    {

        public partial class listthongtin
        {
            public List<thongtinsanpham1> thongtinsanpham1s { get; set; }
        }

        public partial class thongtinsanpham1
        {
            public int idSanpham { get; set; }
            public string Namesanpham { get; set; }
            public DateTime oderDate { get; set; }
            public string? Status { get; set; }
            public float Price { get; set; }
            public int Soluong { get; set; }

        }
            
        
    }
}
