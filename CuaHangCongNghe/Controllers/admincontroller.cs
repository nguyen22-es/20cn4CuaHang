using CuaHangCongNghe.Controllers.laydulieu;
using CuaHangCongNghe.Models.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static CuaHangCongNghe.Controllers.thongtinsanpham;

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

                var categories = db.Users.ToList();

                userViewModel.Users = categories;

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
     public thongtinsanpham GetThongtinsanpham()
        {
            using (var db = new storeContext())
            {
                var thongtin = listthongtin();
            }
        }
        public IActionResult thongtindonhang(int id)
        {
            using (var db = new storeContext())

            {
              
                var oderitemlist = new oderitem();

                var oderlist = db.Orderitems.ToList();
              

                var oderuser = db.Orders.Where(c => c.UserId ==id).FirstOrDefault();

                if (oderuser != null )
                {
                   var tensp = db.Orderitems.Where(c => c.OrderId ==oderuser.OrderId).FirstOrDefault();
                    if (tensp != null)
                    {
                        

                    }



                }
                if (userdangnhap != null && user != null)
                {
                    update.NameUser = user.NameUser;
                    update.EmailUser = user.EmailUser;
                    update.AddressUser = user.AddressUser;
                    update.PhoneUser = user.PhoneUser;
                    update.tendangnhap = userdangnhap.Tendangnhap;
                    update.password = userdangnhap?.Password;
                    return View(update);
                }
                return new RedirectResult(url: "/Home/Index");
            }
        }       
        public partial class userViewModel
        {
            public List<User> Users { get; set; }

        }

        public partial class oderitem
        {
            public List<oderitem> Oderitems { get; set; }
        }
      
    }  
}
