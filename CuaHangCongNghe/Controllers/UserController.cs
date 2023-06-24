using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using CuaHangCongNghe.Controllers.laydulieu;
using CuaHangCongNghe.Models.son1;

namespace CuaHangCongNghe.Controllers;
[AllowAnonymous]
public class UserController : Controller
{


    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;

    }

    public async Task<IActionResult> Login(dangnhapview dangnhap)
    {
        if (ModelState.IsValid)
        {
            using (var db = new storeContext())
            {

                var user = db.Dangnhapusers.FirstOrDefault(c => c.Tendangnhap == dangnhap.NameUser & c.Password == dangnhap.Password);

                if (user != null)
                {
                    

                    var namerole = db.Nameroles.FirstOrDefault(c => c.Idrole == user.Idrole);
                    var claims = new List<Claim>()
            {

                new Claim(ClaimTypes.Name, user.Tendangnhap),
                new Claim(ClaimTypes.Role, namerole.Tenrole),
                new Claim(ClaimTypes.NameIdentifier, user.Iddangnhap.ToString())


                };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);//quản lý xác thực người dùng hiện tại trong ứng dụng,cung cấp những thao tác với danh sách claim
                    var principal = new ClaimsPrincipal(identity);// cung cấp thuộc tính truy xuất thong tin người dùng
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties // 
                    {
                        IsPersistent = true
                    });
                    return LocalRedirect("/Home/Index");

                }
                else
                {

                    ViewBag.dangnhap = "đăng nhập không thành công";
                    return new RedirectResult(url: "/User/dangnhap");
                }
            }


        }
        return new RedirectResult(url: "/User/dangnhap");

    }
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }



    [HttpGet]
    public RedirectResult dangkynew(dangnhapview dangnhapview)
    {

        if (ModelState.IsValid)
        {
            using (var db = new storeContext())
            {
                int i = 0;
                var allUsers = db.Dangnhapusers.ToList();

                foreach (var user in allUsers)
                {
                    if (i == user.Iddangnhap)
                    {
                        i++;
                    }

                }
                var dangnhapuser = db.Dangnhapusers.FirstOrDefault(c => c.Tendangnhap == dangnhapview.NameUser || c.Password == dangnhapview.Password);
                if (dangnhapuser == null)
                {
                    var newdangnhapUser = new Dangnhapuser
                    {
                        Tendangnhap = dangnhapview.NameUser,
                        Password = dangnhapview.Password,
                        Iddangnhap = i,
                        Idrole = 0

                    };
                    db.Dangnhapusers.Add(newdangnhapUser);

                    ViewBag.dangky = "đăng ký thành công!";

                    db.SaveChanges();
                    return new RedirectResult(url: "/User/dangnhap");

                }
                ViewBag.dangky = "tài khoản hoặc mật khẩu đã tôn tại ";
                return new RedirectResult(url: "/User/dangky");
            }

        }
        return new RedirectResult(url: "/User/dangky");

    }

    public IActionResult dangnhap() => View();
    [HttpGet]
    public IActionResult thongtincanhan() 
    {
        using (var db = new storeContext())
        {
           

            // Lấy giá trị của một Claim cụ thể
            string name = User.FindFirstValue(ClaimTypes.Name);// lấy ra tên đang nhập từ claim
            string identifier = User.FindFirstValue(ClaimTypes.NameIdentifier);// lấy ra id đang nhập từ claim để truy xuất đến thông tin người dùng


            var userdangnhap = db.Dangnhapusers.Where(c => c.Tendangnhap == name).FirstOrDefault();

            var user = db.Users.Where(c => c.Iddangnhap == int.Parse(identifier)).FirstOrDefault();

            var thongtinnguoidung = new dangky();// khai báo biến thongtinnguoidung có kiểu là đang ký để lưu ngững thông tin cần thiết của người dùng và hiển thị nó ra trang thongtincanhan
            if (userdangnhap != null && user == null)
            {
                thongtinnguoidung.tendangnhap = userdangnhap.Tendangnhap;
                thongtinnguoidung.password = userdangnhap.Password;
                thongtinnguoidung.idrole = userdangnhap.Idrole;
                thongtinnguoidung.iddangnhap = userdangnhap.Iddangnhap;

                return View(thongtinnguoidung);
            }
            if (userdangnhap != null && user != null)
            {
                thongtinnguoidung.NameUser = user.NameUser;
                thongtinnguoidung.EmailUser = user.EmailUser;
                thongtinnguoidung.AddressUser = user.AddressUser;
                thongtinnguoidung.PhoneUser = user.PhoneUser;
                thongtinnguoidung.tendangnhap = userdangnhap.Tendangnhap;
                thongtinnguoidung.password = userdangnhap?.Password;
                thongtinnguoidung.iddangnhap = userdangnhap.Iddangnhap;
                thongtinnguoidung.iduser = user.UserId;
                return View(thongtinnguoidung);
            }
            return new RedirectResult(url: "/Home/Index");
        }
    }
    public async Task<RedirectResult> saveupdate(dangky dangky) //khai báo biến dangky có kiểu là đang ký để nhận vào những dữ liệu mà mình thay đổi hoặc thêm vào trong phần trang thông tin cá nhân
    {

        using (var db = new storeContext())
        {
            int iduser = 0;
            var user1 = db.Users.ToList();

            foreach (var item in user1)
            {
                if (iduser == item.UserId)
                {
                    iduser = iduser + 1;
                }
                else
                {
                    break;
                }
            }
            var user = db.Users.FirstOrDefault(c => c.Iddangnhap == dangky.iddangnhap);
            var matkhau1 = db.Dangnhapusers.FirstOrDefault(c => c.Iddangnhap == dangky.iddangnhap);

            if (user == null)
            {
                db.Users.Add(new User
                {
                    AddressUser = dangky.AddressUser,
                    PhoneUser = dangky.PhoneUser,
                    NameUser = dangky.NameUser,
                    EmailUser = dangky.EmailUser,
                    Iddangnhap = dangky.iddangnhap,
                    RegistrationDate = DateTime.Now,
                    Idrole = 3,
                    UserId = iduser,

                });
                matkhau1.Idrole = 3;
                db.SaveChanges();

              
            }
            else
            {
                var matkhau = db.Dangnhapusers.FirstOrDefault(c => c.Iddangnhap == dangky.iddangnhap);

                user.AddressUser = dangky.AddressUser;
                user.EmailUser = dangky.EmailUser;
                user.PhoneUser = dangky.PhoneUser;
                user.NameUser = dangky.NameUser;
                matkhau.Password = dangky.password;
                user.Idrole = 3;
                matkhau.Idrole = 3;
                db.SaveChanges();
            }

        }


        ClaimsIdentity nameclaims = User.Identity as ClaimsIdentity;//tạo lại claim để thay đổi role người dùng và gửi lại cookie


        Claim claim = new Claim(ClaimTypes.Role, "user1");
        Claim claim1 = new Claim(ClaimTypes.GivenName, dangky.iduser.ToString());

        if (nameclaims != null)
        {

            var name = nameclaims.FindFirst(ClaimTypes.Role);
            if (name != null)
            {
                nameclaims.RemoveClaim(name);
            }
        }

        nameclaims.AddClaim(claim);

        

        var principal = new ClaimsPrincipal(nameclaims);



      await  HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties // 
        {
            IsPersistent = false
        });




        return new RedirectResult(url: "/user/thongtincanhan");
    }






    /*await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return new RedirectResult(url:"/user/dangnhap");*/

    public IActionResult dangky() => View();


    public IActionResult sanphamadd(int id)  // nhận vào biến id để xem sản phẩm mà mình muốn thêm vào odder
    {
        using (var db = new storeContext())
        {
            Product product1 = new Product();


            var product = db.Products.Where(c => c.Id == id).FirstOrDefault();

            product1 = product;



            return View(product1);
        }
    }


    [HttpGet]
    public IActionResult themvaooder(int id, int soluong) // nhận vào id sản phẩm mà mình muốn thêm vào và số lượng mà người dùng mua từ trang sanphamadd và tạo ra 1 cái đơn hàng mới
    {
        using (var db = new storeContext())
        {


            int iduser = 0;
            var user1 = db.Orders.ToList();

            foreach (var m in user1)
            {
                if (iduser == m.OrderId)
                {
                    iduser = iduser + 2;
                }
                else
                {
                    break;
                }
            }



            int a = 0;
            var ids = db.Orderitems.Select(item => item.OrderItemsId).ToList();

            foreach (var m in ids)
            {
                if (a == m)
                {
                    a = a + 1;
                }
                else
                {
                    break;
                }
            }



            string identifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = db.Users.Where(c => c.Iddangnhap == int.Parse(identifier)).FirstOrDefault();
            if (user != null)
            {
                int u = user.UserId;
                //  if (oderitem != null)
                Order donhang = new Order
                {
                    OrderId = iduser,
                    UserId = user.UserId,
                    OrderDate = DateTime.Today,
                    Status = 0,

                };

                db.Orders.Add(donhang);

                var item = new Orderitem
                {
                    OrderItemsId = a,
                    OrderId = iduser,
                    Quantity = soluong,
                    ProductId = id,
                };
                db.Orderitems.Add(item);

                var sp = db.Products.FirstOrDefault(c => c.Id == id);
                if (sp != null)
                {
                    sp.Stockquantity = sp.Stockquantity - soluong;
                }
                db.SaveChanges();
                return new RedirectResult(url: "/user/thongtincanhan");
            }
            else
            {
                return new RedirectResult(url: "/user/thongtincanhan");
            }
        }
    }
    public ActionResult Deleteoder(int id,int sanpham,int soluong) // nhận vào id tìm đến oder muốn xóa, sanpham để truy cập đến sản phẩm mà mình mua vàu trả lại số lượng mà mình đã mua
    {
        using (var db = new storeContext())
        {
            int iduser;
            var oder = db.Orders.FirstOrDefault((c => c.OrderId == id));
            if (oder != null)
            {
                iduser = oder.UserId;

                db.Orders.Remove(oder);
                var product = db.Products.FirstOrDefault(c => c.Id == sanpham);
                if (product != null)
                {
                    product.Stockquantity = product.Stockquantity + soluong;
                }



                db.SaveChanges();

                return RedirectToAction("thongtindonhang", "admin", new { id = iduser });
            }
            else
            {
                return new RedirectResult(url: "/user/thongtincanhan");
            }
           
        }

    }

}
public partial class dangnhapview
{
    public string NameUser { get; set; }

    public string Password { get; set; }

    public int idrole { get; set; }


}


public partial class Product1
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public float Price { get; set; }
    public int? CategoryId { get; set; }
    public string? ImageUrl { get; set; }
    public int? Stockquantity { get; set; }
}