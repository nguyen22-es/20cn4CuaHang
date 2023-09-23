using CuaHangCongNghe.Models;
using CuaHangCongNghe.Repository;
using CuaHangCongNghe.Service;
using CuaHangCongNghe.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Extensions;


namespace CuaHangCongNghe.Controllers
{
    [Authorize(Roles = "AdminAccess")]
    public class AdminController : Controller
    {
        private readonly ProductService productService;
        private readonly oderItemService oderItemService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
      

        public AdminController(ProductService productService, oderItemService oderItemService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.productService = productService;
            this.oderItemService = oderItemService;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult GetAllOrders() => View(oderItemService.GetCurrentAllOrder());

        public IActionResult GetOrder(string id) 
        {
            return View(oderItemService.GetCurrentAllOrder(id));
        }

        public IActionResult ChangeOrderStatus(int id, int status)
        {
            oderItemService.ChangeStatus(id, status);
            return RedirectToAction("GetOrder", new { id = id });
        }

        public IActionResult GetUsers() => View(userManager.Users.ToList());

        public IActionResult CreateUser() => View();

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.nameLogin };
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
            var model = new UserViewModel { Id = user.Id, EmailUser = user.Email, NameUser = user.Name,  PhoneUser = user.PhoneNumber };
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




        public async Task<ActionResult> EditUserRights(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var userRoles = await userManager.GetUsersInRoleAsync(user.Id);
                var allRoles = roleManager.Roles.ToList();
                var model = new RoleManagerClaims
                {
                    name = user.UserName,
                    Role = allRoles
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

        public IActionResult AddProduct() => View();

        [HttpPost]
        public IActionResult AddProduct(ProductViewModel productViewModel)
        {
            productService.Create(productViewModel);
            return RedirectToAction("AddProduct");
        }
    }
}
