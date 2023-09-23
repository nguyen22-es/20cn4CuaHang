using CuaHangCongNghe.Models;
using CuaHangCongNghe.Models.Shop;
using CuaHangCongNghe.Repository;
using CuaHangCongNghe.Service;
using CuaHangCongNghe.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;

namespace Shop.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly UserService  userService;
        private readonly ProductService productService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly oderItemService  oderItemService;
        public CheckOutController(oderItemService oderItemService, UserService userService, ProductService productService, UserManager<ApplicationUser> userManager)
        {
            this.oderItemService = oderItemService;
            this.userManager = userManager;
            this.productService = productService;
            this.userService = userService;
        }

        public IActionResult Thanks()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateItemOrder( int item,int Amount)
        {
            var orderItemViewModel = new OrderItemViewModel();
            ProductViewModel product;
            product = productService.GetProduct(item);
            oderItemService.AddProductToItems(product, userManager.GetUserId(User), Amount);


            return View();


        }
        [HttpPost]
        public IActionResult GetAllOrder(int OrderId)
        {
            List<OrderItemViewModel> orderItemViewModel;

            orderItemViewModel = oderItemService.GetOrderItem(OrderId);

            return View(orderItemViewModel);
        }

        [HttpPost]
        public  IActionResult SaveOrder(int OrderId,UserViewModel userViewModel)
        {
         
            userService.AddInformation(userManager.GetUserId(User), userViewModel);
            
            return RedirectToAction("Thanks");
        }
    }
}
