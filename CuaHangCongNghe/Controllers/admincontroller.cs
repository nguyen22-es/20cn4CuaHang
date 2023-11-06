using CuaHangCongNghe.Models;
using CuaHangCongNghe.Repository;
using CuaHangCongNghe.Service;
using CuaHangCongNghe.viewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CuaHangCongNghe.Extensions;


namespace CuaHangCongNghe.Controllers
{
  //  [Authorize = "AdminAccess"]
    public class AdminController : Controller
    {
        private readonly IProductService productService;
        private readonly IOrderItemService OrderItemService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
       
      

        public AdminController(IProductService productService, IOrderItemService oderItemService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.productService = productService;
            this.OrderItemService = oderItemService;
            this.userManager = userManager;
            this.roleManager = roleManager;
           
        }
        [HttpGet]
        public IActionResult statistical()
        {
            var statistical = new statistical();

            var allOrderPay = OrderItemService.GetAllOrdernPay();

            foreach(var i in allOrderPay)
            {
                if (statistical.Sta.ContainsKey(i.OrderDate.ToString("dd")))
                {
                    statistical.Sta[i.OrderDate.ToString("dd")] += i.FullPrice; 
            
                }
                else
                {
                   
                    statistical.Sta.Add(i.OrderDate.ToString("dd"), i.FullPrice);
                }
                     
            }
            return Json(statistical);

        }
        public async Task<IActionResult> GetOrders()
        {
            var ListUserViewModelOrder = new List<UserOrderViewModel>();

           
            var orders = OrderItemService.GetCurrentAllOrder();

            for (int i = 0; i < orders.Count; i++)
            {
                var l = new UserOrderViewModel();
                var user = await userManager.FindByIdAsync(orders[i].UserId);

                if (user != null)
                {
                    l.Order = orders[i];
                    l.User = new UserViewModel { Id = user.Id, EmailUser = user.Email, NameUser = user.UserName };
                    ListUserViewModelOrder.Add(l);
                }
            }

            return View(ListUserViewModelOrder);
        }

        public async Task<IActionResult> GetOrder(int id)
        {
  

            var orders = OrderItemService.GetOrder(id);


            return View(orders);
        }
        public IActionResult ChangeOrderStatus(int id, int status)
        {
            OrderItemService.ChangeStatus(id, status);
            return RedirectToAction("GetOrder", new { id = id });
        }

        public IActionResult GetUsers() => View(userManager.Users.ToList());

        public IActionResult CreateUser() => View();

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { Name = model.nameLogin };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("GetUsers");
                }
                else
                {
                    result.AddErrorsTo(ModelState);
                }
            }
            return View(model);
        }
        

        public async Task<IActionResult> GetUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var model = new UserViewModel { Id = user.Id, EmailUser = user.Email, NameUser = user.Name,  PhoneUser = user.PhoneNumber,RegistrationDate = user.DateTime };
            return View(model);
        }

        public async Task<IActionResult> ChangeUserPassword(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var model = new UserViewModel { Id = user.Id, NameUser = user.Name };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserPassword(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var result = await userManager.ChangePasswordAsync(user, model.PasswordConfirm, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetUser", new { id = model.Id });
                    }
                    else
                    {
                        result.AddErrorsTo(ModelState);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "nghười dùng không tồn tại");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var model = new UserViewModel { Id = user.Id, EmailUser = user.Email, NameUser = user.UserName, PhoneUser = user.PhoneNumber };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.EmailUser;
                    user.UserName = model.NameUser;
                    user.PhoneNumber = model.PhoneUser;
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetUser", new { id = model.Id });
                    }
                    else
                    {
                        result.AddErrorsTo(ModelState);
                    }
                }
            }
            return View(model);
        }

        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    result.AddErrorsTo(ModelState);
                }
            }
            return RedirectToAction("GetUsers");
        }

        public IActionResult GetRoles() => View(roleManager.Roles.ToList());

        public async Task<ActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    result.AddErrorsTo(ModelState);
                }
            }
            return RedirectToAction("GetRoles");
        }

        [HttpPost]
        public async Task<ActionResult> CreateRole(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("GetRoles");
                }
                else
                {
                    result.AddErrorsTo(ModelState);
                }
            }
            return View();
        }

        public IActionResult CreateRole() => View();

        public async Task<ActionResult> EditUserRights(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var allRoles = roleManager.Roles.ToList();
                var model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    AllRoles = allRoles,
                    UserRoles = userRoles,
                };
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> EditUserRights(string id, List<string> roles)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);
                var result = await userManager.AddToRolesAsync(user, addedRoles);
                if (!result.Succeeded)
                {
                    result.AddErrorsTo(ModelState);
                    return RedirectToAction("GetUser", new { id = id });
                }
                result = await userManager.RemoveFromRolesAsync(user, removedRoles);
                if (!result.Succeeded)
                {
                    result.AddErrorsTo(ModelState);
                }
                return RedirectToAction("GetUser", new { id = id });
            }
            return NotFound();
        }

        public IActionResult EditProduct(int Id)
        {
            var Product = productService.GetProduct(Id);
            return View(Product);
        }
        [HttpPost]
        public IActionResult EditProducts(ProductViewModel productViewModel)
        {

            productService.Update(productViewModel);



            return RedirectToAction("AllProduct");
        }

        public IActionResult AllProduct()
        {

         var allProduct =   productService.GetAllProducts();


         return View(allProduct);
        }
        public IActionResult AddProduct() => View();

        [HttpPost]
        public IActionResult AddProduct(ProductViewModel productViewModel)
        {
            productService.Create(productViewModel);
            return RedirectToAction("UpdateOrder");
        }
    }
}
