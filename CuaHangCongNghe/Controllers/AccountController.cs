using CuaHangCongNghe.Models;
using CuaHangCongNghe.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem người dùng tồn tại với tên đăng nhập
                var user = await userManager.FindByNameAsync(model.NameLogin);

                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }

                ModelState.AddModelError("NameLogin", "Tên đăng nhập hoặc mật khẩu không đúng");
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Name = model.nameLogin,
                 
                   
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                    // Đăng nhập người dùng sau khi đăng ký (tùy chọn)
                    await signInManager.SignInAsync(user, isPersistent: false);
               
                    return RedirectToAction("Index", "Home"); // Chuyển hướng sau khi đăng ký thành công
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("NameLogin", error.Description);
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
