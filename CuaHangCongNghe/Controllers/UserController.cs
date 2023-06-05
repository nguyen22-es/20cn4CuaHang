
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CuaHangCongNghe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using CuaHangCongNghe.Models.Tables;



namespace CuaHangCongNghe.Controllers;
[AllowAnonymous]
public class UserController : Controller
    {
    private static List<int> n = new List<int>();
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
                var user = db.Users.FirstOrDefault(c => c.NameUser == dangnhap.NameUser & c.Password == dangnhap.Password);

                if (user != null)
                {
                    var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.NameUser),
                new Claim(ClaimTypes.Role, user.Role),
            };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);//quản lý xác thực người dùng, ClaimsIdentity đại diện cho danh tính của người dùng và lưu trữ các khẳng định (claims) về người dùng như tên, vai trò, thông tin xác thực, và các thông tin khác
                    var principal = new ClaimsPrincipal(identity);// cung cấp thuộc tính truy xuất thong tin người dùng
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                    {
                        IsPersistent = true
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

    public IActionResult dangky(dangky dangky)
    {
  
        if (ModelState.IsValid)
        {
            using (var db = new storeContext())
            {
                Random rnd = new Random();
                int random = rnd.Next(0, 100);
                while (n.Contains(random))
                {
                    rnd = new Random();
                    random = rnd.Next(0, 100);

                }
                n.Add(random);
                var user = db.Users.FirstOrDefault(c => c.NameUser==dangky.NameUser && c.Password == dangky.Password);
                if (user == null)
                {
                    var newUser = new User
                    {
                        Role = "user",
                        UserId = random,
                        NameUser = dangky.NameUser,
                        Password = dangky.Password,
                        EmailUser = dangky.EmailUser,
                        AddressUser = dangky.AddressUser,
                        PhoneUser = dangky.PhoneUser
                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    ViewBag.Message = "mời đăng nhập!";
                    return new RedirectResult(url: "/User/dangnhap");
                }
                ViewBag.Message = "tài khoản hoặc mật khẩu đã tôn tại ";
                return new RedirectResult(url: "/User/dangky");
            }
            
        }
        return View();
    } 
        public IActionResult dangnhap() => View();
        public IActionResult thongtincanhan() => View();

    
}
public partial class dangnhapview
{

    public string NameUser { get; set; }

    public string Password { get; set; }


}


