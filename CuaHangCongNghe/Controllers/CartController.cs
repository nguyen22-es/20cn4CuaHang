using CuaHangCongNghe.Repository;
using CuaHangCongNghe.Service;
using CuaHangCongNghe.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Shop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
       
        private readonly ProductService productService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly oderItemService oderItemService;
        private readonly UserService userService;

        public CartController(oderItemService oderItemService, ProductService productService, UserManager<ApplicationUser> userManager, UserService userService)
        {
            this.oderItemService = oderItemService;
            this.productService = productService;
            this.userManager = userManager;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            var orderViewModel = oderItemService.GetOrderPay(userManager.GetUserId(User));
            return View(orderViewModel);
        }

        public IActionResult Add(int id)
        {
            
            var idUser = userManager.GetUserId(User);

            var product = productService.GetProduct(id);

           if( userService.GetUser(idUser) == null)
            {
                userService.createUser(idUser);
            }
           
            oderItemService.AddCart(product, userManager.GetUserId(User));
            return RedirectToAction("Index");
        }

      /*  public IActionResult Update(int cartItemId, int quantity)
        {
            string filePath = @"D:\19paymentUrl.txt";
            string userString = $"Namdfsde: {cartItemId}, Emsdfail: {quantity}";
            System.IO.File.WriteAllText(filePath, userString);
            oderItemService.UpdateQuantity(userManager.GetUserId(User), cartItemId, quantity);
            
            
            return RedirectToAction("Index");
        }*/

        public IActionResult Update(Dictionary<int, int> items)
        {
            string filePath = @"D:\19paymentUrl.txt";
         

            foreach (var item in items)
            {
                string userString = $"Name: {item.Key}, Email: {item.Value}";
                System.IO.File.WriteAllText(filePath, userString);
                oderItemService.UpdateQuantity(userManager.GetUserId(User), item.Key, item.Value);
            }

            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(int itemId)
        {
            oderItemService.DeleteItem(userManager.GetUserId(User), itemId);
            return RedirectToAction("Index");
        }

    }
}
