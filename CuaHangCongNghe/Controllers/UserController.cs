using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using CuaHangCongNghe.Models.Tables;
using CuaHangCongNghe.Controllers.laydulieu;

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
                var namerole = db.Nameroles.FirstOrDefault(c => c.Idrole == user.Idrole);
                if (user != null)
                {
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
                        IsPersistent = false
                    });
                    return LocalRedirect("/Home/Index");

                }
                else
                {

                    ViewBag.Message = "đăng nhập không thành công";
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
                var dangnhapuser = db.Dangnhapusers.FirstOrDefault(c => c.Tendangnhap == dangnhapview.NameUser && c.Password == dangnhapview.Password);
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

                    ViewBag.Message = "đăng ký thành công!";

                    db.SaveChanges();
                    return new RedirectResult(url: "/User/dangnhap");

                }
                ViewBag.Message = "tài khoản hoặc mật khẩu đã tôn tại ";
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
            string name = User.FindFirstValue(ClaimTypes.Name);
            string identifier = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var userdangnhap = db.Dangnhapusers.Where(c => c.Tendangnhap == name).FirstOrDefault();

            var user = db.Users.Where(c => c.Iddangnhap == int.Parse(identifier)).FirstOrDefault();

            var update = new dangky();
            if (userdangnhap != null && user == null)
            {
                update.tendangnhap = userdangnhap.Tendangnhap;
                update.password = userdangnhap?.Password;
                update.idrole = userdangnhap.Idrole;
                update.iddangnhap = userdangnhap.Iddangnhap;

                return View(update);
            }
            if (userdangnhap != null && user != null)
            {
                update.NameUser = user.NameUser;
                update.EmailUser = user.EmailUser;
                update.AddressUser = user.AddressUser;
                update.PhoneUser = user.PhoneUser;
                update.tendangnhap = userdangnhap.Tendangnhap;
                update.password = userdangnhap?.Password;
                update.iddangnhap = userdangnhap.Iddangnhap;
                update.iduser = user.UserId;
                return View(update);
            }
            return new RedirectResult(url: "/Home/Index");
        }
    }
    public async Task<RedirectResult> saveupdate(dangky dangky)
    {
        if (ModelState.IsValid)
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
                        Idrole = dangky.idrole,
                        UserId = iduser,
                    });
                    db.SaveChanges();
                    return new RedirectResult(url: "/user/thongtincanhan");
                }
                else
                {
                    var matkhau = db.Dangnhapusers.FirstOrDefault(c => c.Iddangnhap == dangky.iddangnhap);

                    user.AddressUser = dangky.AddressUser;
                    user.EmailUser = dangky.EmailUser;
                    user.PhoneUser = dangky.PhoneUser;
                    user.NameUser = dangky.NameUser;
                    matkhau.Password = dangky.password;
                    user.Idrole = dangky.idrole;
                    matkhau.Idrole = dangky.idrole;
                    db.SaveChanges();
                }
            }


            ClaimsIdentity nameclaims = User.Identity as ClaimsIdentity;


            Claim claim = new Claim(ClaimTypes.Role, "user1");
            Claim claim1 = new Claim(ClaimTypes.GivenName, dangky.tendangnhap);

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



            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties // 
            {
                IsPersistent = false
            });



        }
        return new RedirectResult(url: "/user/thongtincanhan");
    }






    /*await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return new RedirectResult(url:"/user/dangnhap");*/

    public IActionResult dangky() => View();


    public IActionResult sanphamadd(int id)
    {
        using (var db = new storeContext())
        {
            Product1 product1 = new Product1();


          var  product = db.Products.ToList();

            foreach (var item in product)
            {

            }
      

            return View(product);
        }
    }


    [HttpGet]
    public IActionResult themvaooder(int id, int soluong)
    {
        using (var db = new storeContext())
        {
         
            string identifier = User.FindFirstValue(ClaimTypes.GivenName);

            var user = db.Users.Where(c => c.NameUser == identifier).FirstOrDefault();
            //  if (oderitem != null)
            var donhang = new Order
            {
                UserId = user.UserId,
                OrderDate = DateTime.Today,
                Status = "đang xử lý",

            };
            db.Orders.Add(donhang);

            var item = new Orderitem
            {
                OrderId = donhang.OrderId,
                Quantity = soluong,
                ProductId = id,
            };
            db.Orderitems.Add(item);
            db.SaveChanges();


            return RedirectToAction("thongtindonhang", user.UserId);
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