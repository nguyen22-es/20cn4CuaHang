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

            OrderInfo order = new OrderInfo();
            //Save order to db
            order.OrderId = userOrderViewModel.Order.Id; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            order.Amount = userOrderViewModel.Order.FullPrice;//gia; //gia                    // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND 
            order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending"
            order.CreatedDate = DateTime.Now;
            order.kh_diachi = userOrderViewModel.User.AddressUser;
            order.kh_email = userOrderViewModel.User.EmailUser;
            order.kh_ten = userOrderViewModel.User.NameUser;
            order.kh_dienthoai = userOrderViewModel.User.PhoneUser;


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
            pay.AddRequestData("vnp_OrderInfo", order.OrderId.ToString());
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());
            string paymentUrl = pay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return paymentUrl;
        }


        public bool compare(VnPayCallbackData parsedQuery, string secretKey)
        {
            System.Text.StringBuilder dataBuilder = new System.Text.StringBuilder();
            dataBuilder.Append("vnp_Amount=").Append(parsedQuery.vnp_Amount)
                .Append("&vnp_BankCode=").Append(parsedQuery.vnp_BankCode)
                .Append("&vnp_BankTranNo=").Append(parsedQuery.vnp_BankTranNo)
                .Append("&vnp_CardType=").Append(parsedQuery.vnp_CardType)
                .Append("&vnp_OrderInfo=").Append(parsedQuery.vnp_OrderInfo)
                .Append("&vnp_PayDate=").Append(parsedQuery.vnp_PayDate)
                .Append("&vnp_ResponseCode=").Append(parsedQuery.vnp_ResponseCode)
                .Append("&vnp_TmnCode=").Append(parsedQuery.vnp_TmnCode)
                .Append("&vnp_TransactionNo=").Append(parsedQuery.vnp_TransactionNo)
                .Append("&vnp_TransactionStatus=").Append(parsedQuery.vnp_TransactionStatus)
                .Append("&vnp_TxnRef=").Append(parsedQuery.vnp_TxnRef);
            string data = dataBuilder.ToString();
            var vnp_SecureHash = parsedQuery.vnp_SecureHash;

            string secureHash = Utils.HmacSHA512(secretKey, data);

            bool isValid = string.Equals(secureHash, vnp_SecureHash, StringComparison.OrdinalIgnoreCase);
            return isValid;

        }



    }
}
