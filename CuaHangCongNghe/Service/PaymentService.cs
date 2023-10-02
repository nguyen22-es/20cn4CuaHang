using CuaHangCongNghe.laydulieu;
using CuaHangCongNghe.viewModel;
using CuaHangCongNghe.Controllers.laydulieu;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CuaHangCongNghe.Models.Shop;
using log4net;

namespace CuaHangCongNghe.Service
{
    public class PaymentService : Controller
    {
        private readonly OderItemRepository oderItemRepository;
        private readonly IConfiguration _configuration;

        public PaymentService(OderItemRepository oderItemRepository, IConfiguration configuration)
        {
            this.oderItemRepository = oderItemRepository;
            _configuration = configuration;
        }



        public string PaymentCall(UserOrderViewModel userOrderViewModel)
        {

            // Lấy thông tin cấu hình
            string vnp_Returnurl = _configuration["vnp_Returnurl"];
            string vnp_Url = _configuration["vnp_Url"];
            string vnp_TmnCode = _configuration["vnp_TmnCode"];
            string vnp_HashSecret = _configuration["vnp_HashSecret"];


            VnPayLibrary pay = new VnPayLibrary();
            pay.AddRequestData("vnp_Bill_Mobile", userOrderViewModel.User.PhoneUser.Trim());
            pay.AddRequestData("vnp_Bill_Email", userOrderViewModel.User.EmailUser.Trim());
            if (!String.IsNullOrEmpty(userOrderViewModel.User.NameUser))
            {
                var indexof = userOrderViewModel.User.NameUser.IndexOf(' ');
                pay.AddRequestData("vnp_Bill_FirstName", userOrderViewModel.User.NameUser.Substring(0, indexof));
                pay.AddRequestData("vnp_Bill_LastName", userOrderViewModel.User.NameUser.Substring(indexof + 1, userOrderViewModel.User.NameUser.Length - indexof - 1));
            }
            pay.AddRequestData("vnp_Bill_Address", userOrderViewModel.User.AddressUser.Trim());
            pay.AddRequestData("vnp_Version", VnPayLibrary.VERSION); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", vnp_TmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", (userOrderViewModel.Order.FullPrice * 100).ToString());
            //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", "VND");
            pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(HttpContext));
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_OrderInfo", userOrderViewModel.Order.Id.ToString());
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());
            string paymentUrl = pay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return paymentUrl;
        }


      

    }
}
