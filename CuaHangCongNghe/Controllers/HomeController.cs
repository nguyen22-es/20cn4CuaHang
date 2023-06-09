using System.Diagnostics;
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
        using (var db = new storeContext())
        {
            var Listsanpham = new listsanpham();

            var sanpham = db.Products.ToList();

            Listsanpham.Products = sanpham;

            return View(Listsanpham);
        }
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
    public partial class listsanpham 
    {
    
    public List<Product> Products { get; set; }
    
    
    }
}

