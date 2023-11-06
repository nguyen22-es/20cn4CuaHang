using CuaHangCongNghe.Repository;
using CuaHangCongNghe.Service;
using CuaHangCongNghe.Services;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace CuaHangCongNghe.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
       
        private readonly IProductService productService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrderItemService OrderItemService;
        private readonly IUserService userService;

        public CartController(IOrderItemService OrderItemService, IProductService productService, UserManager<ApplicationUser> userManager, IUserService userService)
        {
            this.OrderItemService = OrderItemService;
            this.productService = productService;
            this.userManager = userManager;
            this.userService = userService;
        }

        public  IActionResult Index()
        {
            var orderViewModel =  OrderItemService.GetOrderPay(userManager.GetUserId(User));
            return View(orderViewModel);
        }

        public  IActionResult Add(int id)
        {
            
            var IdUser = userManager.GetUserId(User);

            
            var product = productService.GetProduct(id);
            var user  = userService.GetUserl(IdUser);
           
            if (user == null)
            {
                userService.createUser(IdUser);
            }
           
            OrderItemService.AddCart(product, IdUser);
            return RedirectToAction("Index");
        }

        public  IActionResult Update(Dictionary<int, int> items)
        {
            var user =  userManager.GetUserId(User);



            foreach (var item in items)
            {
               
                OrderItemService.UpdateQuantity(user, item.Key, item.Value);
            }

            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(int itemId)
        {
            OrderItemService.DeleteItem(userManager.GetUserId(User), itemId);
            return RedirectToAction("Index");
        }

    }
}
