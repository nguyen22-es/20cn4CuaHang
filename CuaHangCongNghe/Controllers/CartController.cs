using CuaHangCongNghe.Repository;
using CuaHangCongNghe.Service;
using CuaHangCongNghe.Services;
using Microsoft.AspNetCore.Authorization;
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
      

        public CartController(oderItemService oderItemService, ProductService productService, UserManager<ApplicationUser> userManager)
        {
            this.oderItemService = oderItemService;
            this.productService = productService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(oderItemService.GetCurrentAllOrder(userManager.GetUserId(User)));
        }

        public IActionResult Add(int id,int amount)
        {
            var product = productService.GetProduct(id);
            oderItemService.AddProductToItems(product, userManager.GetUserId(User), amount);
            return RedirectToAction("Index");
        }

        public IActionResult Update(int cartItemId, int amounts)
        {
          
                oderItemService.UpdateAmount(userManager.GetUserId(User), cartItemId, amounts);
            
            
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(int itemId)
        {
            oderItemService.DeleteItem(userManager.GetUserId(User), itemId);
            return RedirectToAction("Index");
        }

    }
}
