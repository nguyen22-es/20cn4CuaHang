﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CuaHangCongNghe.Models;
using Microsoft.AspNetCore.Authorization;

using CuaHangCongNghe.Models.Tables;

namespace CuaHangCongNghe.Controllers;
[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
   
   

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
public partial class lisuser
{
    public List<User> Users { get; set; }
}

