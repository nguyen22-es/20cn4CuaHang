﻿

using CuaHangCongNghe.Models.Tables;
using Microsoft.AspNetCore.Mvc;

namespace CuaHangCongNghe.Controllers.laydulieu
{



    
    public partial class thongtinsanpham
    {
        public int oderid { get; set; }
        public int idDonHang { get; set; }
        public int idHang { get; set; }
        public string Namesanpham { get; set; }
        public DateTime oderDate { get; set; }
        public string? Status { get; set; }
        public float Price { get; set; }
        public int Soluong { get; set; }
        public int iduser { get; set; }
    }
            
        
    
}
