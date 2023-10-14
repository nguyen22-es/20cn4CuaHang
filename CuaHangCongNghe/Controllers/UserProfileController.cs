using CuaHangCongNghe.Models;
using CuaHangCongNghe.Repository;
using CuaHangCongNghe.Service;
using CuaHangCongNghe.viewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Extensions;


namespace CuaHangCongNghe.Controllers
{
   // [Authorize(Policy = "UserAccess")]
    public class UserProfileController : Controller
    {
    
        private readonly UserManager<ApplicationUser> userManager;
        private readonly OrderItemService  OrderItemService;
        public UserProfileController(UserManager<ApplicationUser> userManager, OrderItemService OrderItemService)
        {
            this.userManager = userManager;
            this.OrderItemService = OrderItemService;
           
        }

        public async Task<IActionResult> Index()
        {
            var user = await userManager.FindByIdAsync(userManager.GetUserId(User));
            var model = new UserViewModel { Id = user.Id, EmailUser = user.Email, NameUser = user.UserName, PhoneUser = user.PhoneNumber,RegistrationDate = user.DateTime,AddressUser = user.Address };
            return View(model);

          
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var model = new UserViewModel { Id = user.Id, EmailUser = user.Email, NameUser = user.UserName, PhoneUser = user.PhoneNumber, RegistrationDate = user.DateTime, AddressUser = user.Address };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
           
        

           
           
            if (model != null)
            {
                var user = await userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    
                    user.Email = model.EmailUser;
                    user.UserName = model.NameUser;
                    user.PhoneNumber = model.PhoneUser;
                    user.Address = model.AddressUser;                   
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        result.AddErrorsTo(ModelState);
                    }
                }
           }
            return View(model);
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var model = new UserViewModel { Id = user.Id, EmailUser = user.Email, NameUser = user.UserName, PhoneUser = user.PhoneNumber, RegistrationDate = user.DateTime, AddressUser = user.Address };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserViewModel model)
        {
            
            if (ModelState.IsValid)
            {                             
                var user = await userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var result = await userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        result.AddErrorsTo(ModelState);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "vui lòng nhập thông tin");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> GetAllOrders()
        {
            var user = await userManager.FindByIdAsync(userManager.GetUserId(User));

            var model = new UserViewModel();

            model.Id = user.Id;

            var Orders = new UserOrdersViewModel();
            if(user != null)
            {
                Orders.Orders = OrderItemService.GetCurrentAllOrderUser(user.Id);
               Orders.User = model;
               
            }
           
            return View(Orders);
        }

        public async Task<ActionResult> GetItemOrder(int id)
        {
            var user = await userManager.FindByIdAsync(userManager.GetUserId(User));
            var model = new UserViewModel { Id = user.Id, EmailUser = user.Email, NameUser = user.UserName, PhoneUser = user.PhoneNumber, RegistrationDate = user.DateTime, AddressUser = user.Address };
            var items = new UserOrderViewModel();
            var order = OrderItemService.GetOrder(id);

            items.Order = order;
            items.User = model;


            return View(items);
        }
    }
}
