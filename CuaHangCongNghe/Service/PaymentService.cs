using Azure.Core;
using CuaHangCongNghe.laydulieu;
using CuaHangCongNghe.viewModel;
using log4net;
using System.Web;

namespace CuaHangCongNghe.Service
{
    public class PaymentService : IPaymentService
    {

        private readonly IConfiguration _configuration;
    
        private readonly oderItemService _oderItemService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(IConfiguration configuration, oderItemService oderItemService, IHttpContextAccessor httpContextAccessor)
        {
            _oderItemService = oderItemService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        
        }



        public  string PaymentCall(UserOrderViewModel userOrderView)
        {

            // Lấy thông tin cấu hình

            string filePath = @"D:\25paymentUrl.txt";





            string vnp_Returnurl = _configuration["vnp_Returnurl"];
            string vnp_Url = _configuration["vnp_Url"];
            string vnp_TmnCode = _configuration["vnp_TmnCode"];
            string vnp_HashSecret = _configuration["vnp_HashSecret"];


            VnPayLibrary pay = new VnPayLibrary();
            pay.AddRequestData("vnp_Bill_Mobile", userOrderView.User.PhoneUser.ToString().Trim());
            pay.AddRequestData("vnp_Bill_Email", userOrderView.User.EmailUser.Trim());

            pay.AddRequestData("vnp_Bill_Address", userOrderView.User.AddressUser.Trim());
            pay.AddRequestData("vnp_Version", VnPayLibrary.VERSION); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", vnp_TmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", (userOrderView.Order.FullPrice * 100).ToString());
            //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", "VND");
            pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(_httpContextAccessor.HttpContext));
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_OrderInfo", userOrderView.Order.Id.ToString());
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());
            string paymentUrl = pay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            string userString = $"phone: {userOrderView.User.PhoneUser}, Email: {userOrderView.User.EmailUser} , url:{paymentUrl}";

            System.IO.File.WriteAllText(filePath, userString);
            return paymentUrl;
        }

        public List<string> CheckPaymentStatus(string IdUser, string urlResponse)
        {
            string filePath = @"D:\28paymentUrl.txt";

          //  string userString = $"phone: {userOrderView.User.PhoneUser}, Email: {userOrderView.User.EmailUser} , url:{paymentUrl}";

            System.IO.File.WriteAllText(filePath, urlResponse);

            var  returns = new List<string>();

            // Kiểm tra nếu URL phản hồi rỗng.
            if (string.IsNullOrEmpty(urlResponse))
            {
                returns.Add("RspCode:99,Message: Dữ liệu đầu vào cần thiết");

                return returns;
            }

            // Tách URL phản hồi thành chuỗi truy vấn.
         
            var vnpayData = HttpUtility.ParseQueryString(urlResponse);

            // Kiểm tra nếu chuỗi truy vấn rỗng.
            if (vnpayData.Count == 0)
            {
                returns.Add( "RspCode:97,Message:Chữ ký không hợp lệ");
                return returns;
            }

            // Lấy bí mật băm VNPAY từ cấu hình.
            string vnp_HashSecret = _configuration["vnp_HashSecret"];

            // Tạo đối tượng VnPayLibrary mới.
            VnPayLibrary vnpay = new VnPayLibrary();

            // Thêm tất cả các tham số chuỗi truy vấn vào đối tượng VnPayLibrary.
            foreach (string s in vnpayData)
            {
                if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(s, vnpayData[s]);
                }
            }

            // Lấy dữ liệu phản hồi VNPAY.
            long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
            string bankCode = vnpay.GetResponseData("vnp_BankCode");
            string vnp_SecureHash = vnpayData["vnp_SecureHash"];

            // Kiểm tra nếu chữ ký hợp lệ.
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

            // Nếu chữ ký không hợp lệ, trả về phản hồi lỗi.
            if (!checkSignature)
            {
              
                returns.Add("có lỗi xảy ra trong quá trình xử lý");
                return returns;
            }
            if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
            {
                 var order = _oderItemService.GetOrderPay(IdUser);
                 order.Status = 1;
                 _oderItemService.ChangeStatus(order.Id, 1);
                // Thanh toán thành công

                returns.Add("Mã giao dịch thanh toán:" + orderId);
                returns.Add("Mã giao dịch tại VNPAY:" + vnpayTranId);


            }
            else
            {
                // Thanh toán không thành công
                returns.Add("Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode);
                return returns;
            }


            // Lấy đơn hàng từ cơ sở dữ liệu.
            returns.Add("Mã Website (Terminal ID):" + vnpayData["vnp_TmnCode"]);          
            returns.Add("Số tiền thanh toán (VND):" + vnp_Amount + "Đ");
            returns.Add("Ngân hàng thanh toán:" + bankCode);
            returns.Add("ngày giao dịch" + DateTime.Now.ToString("dd/MM/yyyy"));
            return returns;
        }
    }
}
