﻿using CuaHangCongNghe.Repository;
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
       
        private readonly ProductService productService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly OrderItemService oderItemService;
        private readonly UserService userService;

        public CartController(OrderItemService oderItemService, ProductService productService, UserManager<ApplicationUser> userManager, UserService userService)
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

        public  IActionResult Add(int id)
        {
            
            var IdUser = userManager.GetUserId(User);

            
            var product = productService.GetProduct(id);
            var user  = userService.GetUserl(IdUser);
           
            if (user == null)
            {
                userService.createUser(IdUser);
            }
           
            oderItemService.AddCart(product, IdUser);
            return RedirectToAction("Index");
        }

        public IActionResult Update(Dictionary<int, int> items)
        {
           
         

            foreach (var item in items)
            {
               
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
