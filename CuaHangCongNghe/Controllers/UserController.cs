
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CuaHangCongNghe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using CuaHangCongNghe.Models.Tables;
using System;



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
                var user = db.Dangnhapusers.FirstOrDefault(c => c.Tendangnhap == dangnhap.NameUser & c.Password == dangnhap.Password);

                if (user != null)
                {
                    var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Tendangnhap),
                new Claim(ClaimTypes.Role, "user"),
                new Claim(ClaimTypes.NameIdentifier, user.Iddangnhap.ToString())
            

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
                var dangnhapuser = db.Dangnhapusers.FirstOrDefault(c => c.Tendangnhap== dangnhapview.NameUser && c.Password == dangnhapview.Password);
                if (dangnhapuser == null)
                {
                    var newdangnhapUser = new Dangnhapuser
                    {
                        Tendangnhap = dangnhapview.NameUser,
                        Password = dangnhapview.Password,
                        Iddangnhap = i,
                       
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
    public IActionResult thongtincanhan() {
        using  (var db = new storeContext())
        {
            var userClaims = User.Claims;

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
                
                return View(update);                          
            }
            if(userdangnhap != null && user!=null) 
            {
                update.NameUser = user.NameUser;              
                update.EmailUser = user.EmailUser;
                update.AddressUser = user.AddressUser;
                update.PhoneUser  = user.PhoneUser;
                update.tendangnhap = userdangnhap.Tendangnhap;
                update.password = userdangnhap?.Password;
                return View(update);
            }
            return new RedirectResult(url: "/Home/Index");
        }
    }
    public RedirectResult saveupdate(dangky dangky)
    {
        if (ModelState.IsValid)
        {
            using(var db = new storeContext())
            {
                string identifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                dangky.iddangnhap = int.Parse(identifier);
                var dangnhap = db.Dangnhapusers.FirstOrDefault(c => c.Tendangnhap == dangky.tendangnhap);
                var user = db.Users.FirstOrDefault(c => c.Iddangnhap == dangky.iddangnhap);
                if(user != null)
                {
                    user.AddressUser = dangky.AddressUser;
                    user.PhoneUser = dangky.PhoneUser;
                    user.NameUser = dangky.NameUser;
                    user.EmailUser = dangky.EmailUser;
                    dangnhap.Tendangnhap = dangky.tendangnhap;
                    dangnhap.Password = dangky.password;              
                    db.SaveChanges();
                }
                if(user == null)
                {
                    db.Users.Add(new User
                    {
                        AddressUser = dangky.AddressUser,
                        PhoneUser = dangky.PhoneUser,
                        NameUser = dangky.NameUser,
                        EmailUser = dangky.EmailUser,
                        Iddangnhap = dangky.iddangnhap,
                        RegistrationDate =DateTime.Now

                    }) ;
                   db.SaveChanges();
                    return new RedirectResult(url: "/User/thongtincanhan");
                }
            }         
        }
        return new RedirectResult(url: "/User/thongtincanhan");
    }
        public IActionResult dangky() => View();
    
}
public partial class dangnhapview
{
    public string NameUser { get; set; }

    public string Password { get; set; }


}


