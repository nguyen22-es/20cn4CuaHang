
using Microsoft.AspNetCore.Mvc;


namespace CuaHangCongNghe.Controllers;
public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult dangky() => View();
        public IActionResult dangnhap() => View();
        public IActionResult thongtincanhan() => View();
        

}

