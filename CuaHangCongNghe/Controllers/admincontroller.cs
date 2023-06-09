using CuaHangCongNghe.Models.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CuaHangCongNghe.Controllers.laydulieu;


namespace CuaHangCongNghe.Controllers
{
    [AllowAnonymous]
    public class admincontroller : Controller
    {
        private readonly ILogger<admincontroller> _logger;

        public admincontroller(ILogger<admincontroller> logger)
        {
            _logger = logger;
        }

      public IActionResult thongTinNguoiDung()
        {
            using (var db = new storeContext())
            {
                var userViewModel = new userViewModel();

                var user1 = db.Users.ToList();

                userViewModel.Users = user1;

                return View(userViewModel);
            }
           
        }
        public RedirectResult Deleteuser(int id)
        {
            using (var db = new storeContext())
            {
                var user = db.Users.Where(c => c.UserId == id).FirstOrDefault();
                if (user != null)
                {

                    db.Users.Remove(user);
                    db.SaveChanges();
                }
                return new RedirectResult(url: "/admin/thongTinNguoiDung");
            }
        }
        public thongtinsanpham xemlist(int id)
        {
            using (var db = new storeContext())
            {
                var thongtindonhang = new thongtinsanpham();

                var oder = db.Orders.Where(c => c.OrderId == id).FirstOrDefault();
                if (oder != null)
                {
                    thongtindonhang.iduser = oder.UserId;
                    thongtindonhang.oderid = oder.OrderId;
                    thongtindonhang.oderDate = oder.OrderDate;
                    thongtindonhang.Status = oder.Status;
                    thongtindonhang.idDonHang = oder.OrderId;
                }
                else
                {
                    thongtindonhang.oderDate = DateTime.MinValue; // Giá trị mặc định cho oderDate khi oder là null
                    thongtindonhang.Status = string.Empty; // Giá trị mặc định cho Status khi oder là null
                    thongtindonhang.idDonHang = 0; // Giá trị mặc định cho idDonHang khi oder là null
                }

                var oderitems = db.Orderitems.Where(c => c.OrderId == thongtindonhang.idDonHang).FirstOrDefault();
                if (oderitems != null)
                {
                    thongtindonhang.idHang = oderitems.ProductId;
                    thongtindonhang.Soluong = oderitems.Quantity;
                }
                else
                {
                    thongtindonhang.idHang = 0; // Giá trị mặc định cho idHang khi oderitems là null
                    thongtindonhang.Soluong = 0; // Giá trị mặc định cho Soluong khi oderitems là null
                }

                var sanpham = db.Products.Where(c => c.Id == thongtindonhang.idHang).FirstOrDefault();
                if (sanpham != null)
                {
                    thongtindonhang.Namesanpham = sanpham.Name;
                    thongtindonhang.Price = sanpham.Price * thongtindonhang.Soluong;
                }
                else
                {
                    thongtindonhang.Namesanpham = string.Empty; // Giá trị mặc định cho Namesanpham khi sanpham là null
                    thongtindonhang.Price = 0; // Giá trị mặc định cho Price khi sanpham là null
                }

                return thongtindonhang;
            }
        }

        public IActionResult thongtindonhang(int id)
        {
            using (var db = new storeContext())
            {
                var oderlist = new oder();
                var listthongtindonhang = new listoder();
                listthongtindonhang.thongtinsanphams = new List<thongtinsanpham>(); ;
                var listoder = db.Orders.ToList();
                oderlist.Orders = listoder;

                foreach (var item in listoder)
                {
                    if (item.UserId == id)
                    {
                        var thongtindonhang = xemlist(item.OrderId);
                        if (thongtindonhang != null)
                        {
                            listthongtindonhang.thongtinsanphams.Add(xemlist(item.OrderId));
                        }
                    }
                }

                return View(listthongtindonhang);
            }

        }
        [HttpPost]
        public IActionResult saveTrangThai(string name, int id1,int id)
        {
            if (ModelState.IsValid)
            {
                using (var db = new storeContext())
                {
                    var status = db.Orders.FirstOrDefault(c => c.OrderId == id1);
                    if (status != null)
                    {
                        status.Status = name;
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("thongtindonhang",new {id});
        }
        public partial class userViewModel
        {
            public List<User> Users { get; set; }

        }
       
        public partial class oder
        {
            public List<Order>  Orders { get; set; }
        }
        public partial class listoder
        {
            public List<thongtinsanpham> thongtinsanphams { get; set; }
        }

    }

}
