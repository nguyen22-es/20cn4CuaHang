using Microsoft.AspNetCore.Mvc;
using WebApplication4.Controllers;

namespace CuaHangCongNghe.Controllers
{
    public class DanhmucController : Controller
    {
        private readonly ILogger<DanhmucController> _logger;

        public DanhmucController(ILogger<DanhmucController> logger)
        {
            _logger = logger;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult maytinh()
        {
            return View();
        }
    }
}
