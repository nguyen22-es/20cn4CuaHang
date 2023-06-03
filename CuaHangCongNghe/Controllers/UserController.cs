using Microsoft.AspNetCore.Mvc;

namespace CuaHangCongNghe.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
