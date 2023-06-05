using CuaHangCongNghe.Models.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult CreateCategory()
        {
            return View();
        }
        public partial class userViewModel
        {
            public List<User> Users { get; set; }

        }
    }  
}
