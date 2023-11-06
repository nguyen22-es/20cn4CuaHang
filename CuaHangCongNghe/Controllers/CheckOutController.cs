using Azure;
using CuaHangCongNghe.laydulieu;
using CuaHangCongNghe.Models;

using CuaHangCongNghe.Repository;
using CuaHangCongNghe.Service;
using CuaHangCongNghe.Services;
using CuaHangCongNghe.viewModel;
using log4net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace Shop.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly IUserService  userService;
        private readonly IProductService productService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrderItemService  OrderItemService;
        private readonly IPaymentService paymentService;
        private readonly IConfiguration _configuration;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static UserOrderViewModel userOrderViewModel;
        public CheckOutController(IPaymentService paymentService,IOrderItemService oderItemService, IUserService userService, IProductService productService, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.OrderItemService = oderItemService;
            this.userManager = userManager;
            this.productService = productService;
            this.userService = userService;
            _configuration = configuration;
            this.paymentService = paymentService;
        }


        public IActionResult Pay()
        {
         

            return View(userOrderViewModel);
            
        }
     
        public async Task<IActionResult> CreateItemOrder()
        {
            var user = await userManager.FindByIdAsync(userManager.GetUserId(User));

            var  userOrderView = new UserOrderViewModel();
            ProductViewModel product;

         
                userOrderView.Order = OrderItemService.GetOrderPay(user.Id);

      
            

            userOrderView.User = new UserViewModel { Id = user.Id, EmailUser = user.Email, NameUser = user.UserName, PhoneUser = user.PhoneNumber, RegistrationDate = user.DateTime, AddressUser = user.Address };
            userOrderViewModel = userOrderView;
            return View(userOrderView);
        }



        [HttpPost]
        public IActionResult SaveOrder(UserOrderViewModel model)
        {

            userService.AddInformation(userManager.GetUserId(User), model.User);
            

          
            return RedirectToAction("Pay");
        }

         
        [HttpPost]
        [Route("api/v1/payment")]
        public async Task<IActionResult> Payment(string status)
        {
           
                var user = userService.GetUser(userManager.GetUserId(User));
            // PaymentService paymentService =  new PaymentService(_configuration,OrderItemService);
                var userOrderView = new UserOrderViewModel();
                ProductViewModel product;

               
                
                  
               userOrderView.Order = OrderItemService.GetOrderPay(userManager.GetUserId(User));

            


                userOrderView.User = user;

            if (status == "1")
            {

                if (userOrderView != null)
                {
                    var respone = paymentService.PaymentCall(userOrderView);
                    if (!string.IsNullOrEmpty(respone))
                    {
                        return Redirect(respone);
                    }
                }
                return NotFound();
            }
            if(status == "2")
            {
                OrderItemService.ChangeStatus(userOrderView.Order.Id,2);
                foreach(var itemOrder in userOrderView.Order.ItemViewModels)
                {
                    productService.UpdateProduct(itemOrder.Product.Id, itemOrder.quantity);
                }
                
            }

            return RedirectToAction("Index", "Cart");
        }



        [HttpGet]
        public ActionResult ResponseVnp()
        {

            string queryString = Request.QueryString.ToString();

            var response = paymentService.CheckPaymentStatus(userManager.GetUserId(User), queryString);
            return View(response);


        }
    }
}
