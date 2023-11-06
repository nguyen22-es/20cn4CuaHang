
using CuaHangCongNghe.Service;
using CuaHangCongNghe.Services;
using Microsoft.AspNetCore.Mvc;

using System;

namespace CuaHangCongNghe.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        public IActionResult Index(int id)
        {
            var product = productService.GetProduct(id);
            return View(product);
        }
    }
}
