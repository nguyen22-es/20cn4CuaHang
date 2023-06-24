
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CuaHangCongNghe.Controllers.laydulieu;
using static CuaHangCongNghe.Controllers.HomeController;
using CuaHangCongNghe.Models.son1;

namespace CuaHangCongNghe.Controllers
{
    [AllowAnonymous]
    public class admincontroller : Controller
    {
        private readonly ILogger<admincontroller> _logger;

        public admincontroller(ILogger<admincontroller> logger)
        {
            _logger = logger;
        }

        public IActionResult thongTinNguoiDung(string nametim)//hiển thị thông tin người dùng
        {
            using (var db = new storeContext())
            {
                var userViewModel = new userViewModel();
                var user1 = db.Users.ToList();
                userViewModel.Users = new List<User>();
                if (nametim == null)
                {
                    userViewModel.Users = user1;
                }
                else
                {
                    userViewModel.Users = new List<User>();

                    foreach (var user in user1)
                    {
                        bool isAdmin = user.NameUser.IndexOf(nametim, StringComparison.OrdinalIgnoreCase) >= 0;
                        if (isAdmin)
                        {
                            userViewModel.Users.Add(user);
                        }
                    }
                }

                return View(userViewModel);
            }

        }
        public RedirectResult Deleteuser(int id)//chuyền vào id user xóa người dùng theo iduser
        {
            using (var db = new storeContext())
            {
                var user = db.Users.Where(c => c.UserId == id).FirstOrDefault();
                if (user != null)
                {

                    db.Users.Remove(user);
                    db.SaveChanges();
                }
                return new RedirectResult(url: "/admin/thongTinNguoiDung");
            }
        }
        public thongtinsanpham xemlist(int id)//chuyền vào id sản phẩm và lấy thông tin cần thiết gán vào biến thongtindonhang để hiển thị
        {
            using (var db = new storeContext())
            {
                var thongtindonhang = new thongtinsanpham();

                var oder = db.Orders.Where(c => c.OrderId == id).FirstOrDefault();
                if (oder != null)
                {
                    thongtindonhang.iduser = oder.UserId;
                    thongtindonhang.oderid = oder.OrderId;
                    thongtindonhang.oderDate = oder.OrderDate;
                    thongtindonhang.idDonHang = oder.OrderId;
                    thongtindonhang.idstatus = oder.Status;
                    var status = db.Statuses.Where(c => c.StatusId == thongtindonhang.idstatus).FirstOrDefault();
                    if (status != null)
                    {
                       
                        thongtindonhang.Status = status.StatusName;
                    }
                    var oderitems = db.Orderitems.Where(c => c.OrderId == thongtindonhang.idDonHang).FirstOrDefault();
                    if (oderitems != null)
                    {
                        thongtindonhang.idHang = oderitems.ProductId;
                        thongtindonhang.Soluong = oderitems.Quantity;


                        var sanpham = db.Products.Where(c => c.Id == thongtindonhang.idHang).FirstOrDefault();
                        if (sanpham != null)
                        {
                            thongtindonhang.Namesanpham = sanpham.Name;
                            thongtindonhang.Price = sanpham.Price * thongtindonhang.Soluong;
                        }


                    }


                }                                                                     
                return thongtindonhang;
            }
        }

        public IActionResult thongtindonhang(int id)// lấy id người dùng để lấy những đơn hàng của người dùng để hiển thị nó ra view(thongtindonhang)
        {
            using (var db = new storeContext())
            {
                var oderlist = new oder();
                var listthongtindonhang = new listoder();
                listthongtindonhang.thongtinsanphams = new List<thongtinsanpham>(); ;
                var listoder = db.Orders.ToList();
                oderlist.Orders = listoder;

                foreach (var item in listoder)
                {
                    if (item.UserId == id)
                    {
                        var thongtindonhang = xemlist(item.OrderId);
                        if (thongtindonhang != null)
                        {
                            listthongtindonhang.thongtinsanphams.Add(thongtindonhang);
                        }
                    }
                }

                return View(listthongtindonhang);
            }

        }
        [HttpPost]
        public IActionResult saveTrangThai(int name, int id1, int id)//nhận vào id trạng thái đơn hàng id đơn hàng mà mình muốn chỉnh sủa trạng thái, id người dùng để quay lại trang thongtindonhang
        {
            if (ModelState.IsValid)
            {
                using (var db = new storeContext())
                {
                    var status = db.Orders.FirstOrDefault(c => c.OrderId == id1);
                    if (status != null)
                    {
                        status.Status = name;
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("thongtindonhang", new { id });
        }
        public ActionResult themsanpham() // hiển thị những danh mục đã có vào trang themsanpham 
        {

            using (var db = new storeContext())
            {
                var listdanhmuc = new listdanhmuc();

                var user1 = db.Categories.ToList();

                listdanhmuc.Categories = user1;

                return View(listdanhmuc);
            }

        }

        [HttpPost]
        public IActionResult savesanpham(sanpham sanpham, IFormFile image, string name1)//nhận vào 1 ảnh,biến sanpham,biến name1 là tên của category
        {
            if (image != null)
            {
                string fileName = Path.GetFileName(image.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                sanpham.ImageUrl = "images/" + fileName;
            }

            using (var db = new storeContext())
            {
                int h = 0;
                var ctgr = db.Categories.ToList();

                foreach (var c in ctgr)
                {
                    if (c.Id == h)
                    {
                        h = h + 1;
                    }
                }



                var category = db.Categories.FirstOrDefault(c => c.Name == name1);

                if (category != null )
                {
                    var newProduct = new Product
                    {
                        Name = sanpham.Name,
                        Description = sanpham.Description,
                        Price = sanpham.Price,
                        ImageUrl = sanpham.ImageUrl,
                        Stockquantity = sanpham.Stockquantity,
                        CategoryId = category.Id
                    };
                    db.Products.Add(newProduct);
                    db.SaveChanges();
                    return RedirectToAction("product");
                }
                 if (category == null)
                {
                    var newCategory = new Category
                    {
                        Name = name1,
                        Id = h
                    };
                    db.Categories.Add(newCategory);
                    db.SaveChanges();

                    var newProduct = new Product
                    {
                        Name = sanpham.Name,
                        Description = sanpham.Description,
                        Price = sanpham.Price,
                        ImageUrl = sanpham.ImageUrl,
                        Stockquantity = sanpham.Stockquantity,
                        CategoryId = newCategory.Id
                    };
                    db.Products.Add(newProduct);
                    db.SaveChanges();

                    return RedirectToAction("product");
                }
                 
            }

            ViewBag.YourVariable = "thêm không thành công";

            return RedirectToAction("themsanpham");
        }
        [HttpGet]
        public ActionResult product(string nametim)//nhận vào tên sản phẩm tìm kiếm sản phẩm theo tên 
        {

            using (var db = new storeContext())
            {


                var Listsanpham = new listsanpham();
                var nameproduct = db.Products.ToList();
                Listsanpham.Products = new List<Product>();
                if (nametim == null)
                {
                    Listsanpham.Products = nameproduct;
                }
                else
                {
                    Listsanpham.Products = new List<Product>();

                    foreach (var user in nameproduct)
                    {
                        bool isAdmin = user.Name.IndexOf(nametim, StringComparison.OrdinalIgnoreCase) >= 0;
                        if (isAdmin)
                        {
                            Listsanpham.Products.Add(user);
                        }
                    }
                }

                return View(Listsanpham);
            }

        }

        public RedirectResult Deleteproduct(int id) // nhận vào vào id sản phẩm và tìm kiếm nó trong sql để xóa
        {
            using (var db = new storeContext())
            {
                var user = db.Products.Where(c => c.Id == id).FirstOrDefault();
                if (user != null)
                {

                    db.Products.Remove(user);
                    db.SaveChanges();
                }
                return new RedirectResult(url: "/admin/product");
            }
        }
        public ActionResult updateproduct(int id)// nhận vào id sản phẩm để tìm kiếm nó trong csdl rồi hiển thị ra trang updateproduct
        {
            using (var db = new storeContext())
            {
                var product = db.Products.Where(c => c.Id == id).FirstOrDefault();

                return View(product);
            }
        }
        public ActionResult editproduct(Product product)// nhận vào những dữ liệu mà mình nhập vào để thay đổi thông tin của sản phẩm và thay đổi nó
        {
            using (var db = new storeContext())
            {
                var productedit = db.Products.Where(c => c.Id == product.Id).FirstOrDefault();
                if (productedit != null)
                {

                    productedit.Stockquantity = product.Stockquantity;
                    productedit.Price = product.Price;
                    productedit.Description = product.Description;
                    productedit.Name = product.Name;
                    db.SaveChanges();
                }
                return new RedirectResult(url: "/admin/product");
            }
        }


        [HttpGet]
        public ActionResult xacnhan(int id,int user )// nhận vào id order tìm kiếm nó và thay đổi trạng thái cảu nó và id user để quay về trang thông tin dơn hàng của người dùng đó
        {
            using (var db = new storeContext())
            {
                
                var order = db.Orders.FirstOrDefault(c => c.OrderId == id);
                if(order != null)
                {
                  
                    order.Status = 3;
                    db.SaveChanges();
                }
                
                return RedirectToAction("thongtindonhang", new { id = user });
            }
        }
        public partial class userViewModel
        {
            public List<User> Users { get; set; }

        }

        public partial class oder
        {
            public List<Order> Orders { get; set; }
        }
        public partial class listoder
        {
            public List<thongtinsanpham> thongtinsanphams { get; set; }
        }
        public partial class listdanhmuc
        {
            public List<Category> Categories { get; set; }
        }

    }
}


