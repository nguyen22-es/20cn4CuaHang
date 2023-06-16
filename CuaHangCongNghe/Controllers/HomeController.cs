using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CuaHangCongNghe.Models;
using Microsoft.AspNetCore.Authorization;
using CuaHangCongNghe.Models.Tables;
using CuaHangCongNghe.Controllers.laydulieu;

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
    
    public IActionResult maytinh(string name1,string name2,string name3)
    {
        using (var db = new storeContext())
        {
          
            List<Product> Listsanpham1 = new List<Product>();
           
         
            List<Category> categories = new List<Category>();
             categories = db.Categories.ToList();

            var name = db.Categories.FirstOrDefault(c => c.Name == name1);
            if (name != null)
            {
                 var Listsanpham = db.Products.ToList();
                 foreach (var product in Listsanpham)
                 {
                     if (product.CategoryId == name.Id)
                     {
                         Listsanpham1.Add(product);
                     }

                 }
             //   Listsanpham1 = db.Products.ToList();
            }
            else
            {
                Listsanpham1 = db.Products.ToList();
            }
            var result = new Tuple<List<Product>, List<Category> >(Listsanpham1, categories);


            return View(result);
           
        }
    }
   


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public partial class listsanpham 
    {
    
    public List<Product> Products { get; set; }
    public List<Category> Category { get; set; }
    
    }
    public partial class listhienthisp
    {

        public List<hienthisanpham>  hienthisanphams { get; set; }


    }

}

