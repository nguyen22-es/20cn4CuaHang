using CuaHangCongNghe.Models;
using CuaHangCongNghe.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Extensions;

namespace CuaHangCongNghe.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
     

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                // Kiểm tra xem người dùng tồn tại với tên đăng nhập
               
                    var result =  signInManager.PasswordSignInAsync(model.NameLogin, model.Password, model.RememberMe, false).Result;

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                              return Redirect(model.ReturnUrl);
                           // return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    
                }

                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            }
            else
            {
                ModelState.AddModelError("", "Các trường phải được điền đầy đủ");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                   Name = model.nameLogin,
                    DateTime = DateTime.Today,
                     UserName = model.nameLogin,
                    Address = "123 Main St"

                };
                var result = userManager.CreateAsync(user, model.Password).Result;
                if (result.Succeeded)
                {
                    signInManager.SignInAsync(user, false);
                    return RedirectToAction("Login");
                }
                else
                {
                    result.AddErrorsTo(ModelState);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
