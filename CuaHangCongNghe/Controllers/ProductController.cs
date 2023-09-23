﻿
using CuaHangCongNghe.Services;
using Microsoft.AspNetCore.Mvc;

using System;

namespace Shop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService productService;

        public ProductController(ProductService productService)
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
