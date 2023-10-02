using CuaHangCongNghe.laydulieu;
using CuaHangCongNghe.Models;
using System.Linq;
using CuaHangCongNghe.Repository;
using CuaHangCongNghe.Service;
using CuaHangCongNghe.Services;
using CuaHangCongNghe.viewModel;
using log4net;
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
        private readonly PaymentService paymentService;
        private readonly IConfiguration _configuration;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public CheckOutController(oderItemService oderItemService, UserService userService, ProductService productService, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.oderItemService = oderItemService;
            this.userManager = userManager;
            this.productService = productService;
            this.userService = userService;
            _configuration = configuration;
        }

        public IActionResult Thanks()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateItemOrder(Dictionary<int, int> items)
        {
            var user = await userManager.FindByIdAsync(userManager.GetUserId(User));

            var userOrderViewModel = new UserOrderViewModel();
            ProductViewModel product;

            foreach (var item in items)
            {
                product = productService.GetProduct(item.Key);
                oderItemService.AddProductToItems(product, user.Id, item.Value);

            } 
            userOrderViewModel.Order = oderItemService.GetOrderPay(user.Id);
            userOrderViewModel.User = new UserViewModel { Id = user.Id, EmailUser = user.Email, NameUser = user.UserName, PhoneUser = user.PhoneNumber, RegistrationDate = user.DateTime, AddressUser = user.Address };

            return View(userOrderViewModel);
        }


        [HttpPost]
        public IActionResult GetAllOrder(int OrderId)
        {
            List<OrderItemViewModel> orderItemViewModel;

            orderItemViewModel = oderItemService.GetOrderItem(OrderId);

            return View(orderItemViewModel);
        }

        [HttpPost]
        public IActionResult SaveOrder(Dictionary<int, int> items, UserOrderViewModel  userOrderViewModel)
        {
           
            userService.AddInformation(userManager.GetUserId(User), userOrderViewModel.User);
           
            return RedirectToAction("Thanks");
        }
        public IActionResult Status(int orderId, int status)
        {
            oderItemService.ChangeStatus(orderId,status);

            return RedirectToAction("Thanks");
        }

     
        [HttpPost]
        public IActionResult PaymentCall(UserOrderViewModel userOrderViewModel)
        {
          string UrlVnp =  paymentService.PaymentCall(userOrderViewModel);

            return Redirect(UrlVnp);
        }



        [HttpGet]
        public IActionResult ResponseVnp()
        {
            string returnContent = string.Empty;

            // Get VNPAY secret key from configuration
            string vnp_HashSecret = _configuration["vnp_HashSecret"];

            // Get all querystring data
            Dictionary<string, string> vnpayData = Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());

            // Create a new VnPayLibrary object
            VnPayLibrary vnpay = new VnPayLibrary();

            // Add all querystring data to the VnPayLibrary object
            foreach (var item in vnpayData)
            {
                vnpay.AddResponseData(item.Key, item.Value);
            }

            // Validate signature
            bool checkSignature = vnpay.ValidateSignature(Request.Query["vnp_SecureHash"], vnp_HashSecret);

            if (checkSignature)
            {
                // Get VNPAY response data
                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                string bankCode = vnpay.GetResponseData("vnp_BankCode");

                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    // Thanh toán thành công
                    ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                    log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                }
                else
                {
                    // Thanh toán không thành công
                    ViewBag.ErrorMessage = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                }

                // Hiển thị thông tin giao dịch
                ViewBag.InnerTextTerminalID = "Mã Website (Terminal ID):" + Request.Query["vnp_TmnCode"];
                ViewBag.InnerTextorderId = "Mã giao dịch thanh toán:" + orderId;
                ViewBag.InnerTextvnpayTranId = "Mã giao dịch tại VNPAY:" + vnpayTranId;
                ViewBag.InnerTextvnp_Amount = "Số tiền thanh toán (VND):" + vnp_Amount + "Đ";
                ViewBag.InnerTextbankCode = "Ngân hàng thanh toán:" + bankCode;
                ViewBag.date = "ngày giao dịch" + DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra trong quá trình xử lý";
            }

            return View();
        }



    }
}
