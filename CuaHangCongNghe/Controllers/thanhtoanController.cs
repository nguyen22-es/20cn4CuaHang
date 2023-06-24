using log4net;
using Microsoft.AspNetCore.Mvc;
using CuaHangCongNghe.Controllers.laydulieu;
using CuaHangCongNghe.Models.son1;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CuaHangCongNghe.Controllers;


public class thanhtoanController : Controller
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private readonly IConfiguration _configuration;
    private readonly ILogger<thanhtoanController> _logger;

    public thanhtoanController(IConfiguration configuration, ILogger<thanhtoanController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public ActionResult index52()
    {

        return View();
    }
    private string ExecPostRequest(string url, string postData)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";

        using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
        {
            writer.Write(postData);
        }

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }
    }

    private string GetHmacSha256Hash(string message, string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);

        using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
        {
            byte[] hashBytes = hmac.ComputeHash(messageBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
    public bool sosanhw(VnPayCallbackData parsedQuery, string secretKey)
    {
        StringBuilder dataBuilder = new StringBuilder();
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
    public ActionResult pay(int id)
    {
        using (var db = new storeContext())
        {

            string identifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userthanhtoan = db.Users.Where(c => c.Iddangnhap == int.Parse(identifier)).FirstOrDefault();
            var item = db.Orderitems.FirstOrDefault(c => c.OrderId == id);
            hienthisanpham hienthi = new hienthisanpham();

            if (userthanhtoan != null && item != null)
            {
                hienthi.oderid = item.OrderId;
                hienthi.Stockquantity = item.Quantity;
                hienthi.idsanpham = item.ProductId;

                var sanpham = db.Products.Where(c => c.Id == hienthi.idsanpham).FirstOrDefault();
                if (sanpham != null)
                {
                    hienthi.Name = sanpham.Name;
                    hienthi.Price = sanpham.Price * hienthi.Stockquantity;
                }

            }

            User currentUser = userthanhtoan; 

            var result = new Tuple<hienthisanpham, User>(hienthi, currentUser);
            sanphamthanhtoan sanphamthanhtoan = new sanphamthanhtoan();
            sanphamthanhtoan.tuple = result;

            return View(sanphamthanhtoan);




        }

    }
    [HttpPost]
    public ActionResult call(double gia,int oderid, string kh_ten, string kh_diachi, string kh_email, string kh_dienthoai)
    {


        var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

        var configuration = builder.Build();



        // Lấy thông tin cấu hình
        string vnp_Returnurl = configuration["vnp_Returnurl"];
        string vnp_Url = configuration["vnp_Url"];
        string vnp_TmnCode = configuration["vnp_TmnCode"];
        string vnp_HashSecret = configuration["vnp_HashSecret"];

        OrderInfo order = new OrderInfo();
        //Save order to db
        order.OrderId = oderid; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
        order.Amount = gia;//gia; //gia                    // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND 
        order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending"
        order.CreatedDate = DateTime.Now;
        order.kh_diachi = kh_diachi;
        order.kh_email = kh_email;
        order.kh_ten = kh_ten;
        order.kh_dienthoai = kh_dienthoai;


        VnPayLibrary pay = new VnPayLibrary();
        pay.AddRequestData("vnp_Bill_Mobile", order.kh_dienthoai.Trim());
        pay.AddRequestData("vnp_Bill_Email", order.kh_email.Trim());
        if (!String.IsNullOrEmpty(order.kh_ten))
        {
            var indexof = order.kh_ten.IndexOf(' ');
            pay.AddRequestData("vnp_Bill_FirstName", order.kh_ten.Substring(0, indexof));
            pay.AddRequestData("vnp_Bill_LastName", order.kh_ten.Substring(indexof + 1,  order.kh_ten.Length - indexof - 1));
        }
        pay.AddRequestData("vnp_Bill_Address", order.kh_diachi.Trim());
        pay.AddRequestData("vnp_Version", VnPayLibrary.VERSION); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
        pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
        pay.AddRequestData("vnp_TmnCode", vnp_TmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
        pay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString());
        //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
        pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_CurrCode", "VND");
        pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(HttpContext));
        pay.AddRequestData("vnp_OrderType", "other");
        pay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
        pay.AddRequestData("vnp_Locale", "vn");
        pay.AddRequestData("vnp_OrderInfo",  order.OrderId.ToString());
        pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());
        string paymentUrl = pay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
        string filePath = @"D:\10paymentUrl.txt";

        // Ghi giá trị của paymentUrl vào tệp
        System.IO.File.WriteAllText(filePath, paymentUrl);
        return Redirect(paymentUrl);
    }







    [HttpGet]
    public ActionResult PaymentConfirm(VnPayCallbackData parsedQuery)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        var configuration = builder.Build();
      
    string vnp_HashSecret = configuration["vnp_HashSecret"]; // Chuỗi bí mật




        var orderId = parsedQuery.vnp_OrderInfo;
        var vnpayTranId = parsedQuery.vnp_TransactionNo;
        var vnp_ResponseCode = parsedQuery.vnp_ResponseCode;
        var vnp_TransactionStatus = parsedQuery.vnp_TransactionStatus;
        var vnp_SecureHash = parsedQuery.vnp_SecureHash;
        var TerminalID = parsedQuery.vnp_TmnCode;
        var vnp_Amount = parsedQuery.vnp_Amount;
        var bankCode = parsedQuery.vnp_BankCode;
        var vnpTxnRef = parsedQuery.vnp_TxnRef;
       
        bool sosanh1 = sosanhw(parsedQuery, vnp_HashSecret);
        int idoder = int.Parse(orderId);


        if (sosanh1 == true)
        {





            if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
            {
                ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);

                using(var db = new storeContext())
                {
                    var order = db.Orders.FirstOrDefault(c => c.OrderId == idoder);
                    if(order != null)
                    {
                        order.Status = 1;
                    }
                }

                
            }
            else
            {
                //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                ViewBag.ErrorMessage = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;

            }
            ViewBag.InnerTextTerminalID = "Mã Website (Terminal ID):" + TerminalID;
                ViewBag.InnerTextorderId = "Mã giao dịch thanh toán:" + orderId;
                ViewBag.InnerTextvnpayTranId = "Mã giao dịch tại VNPAY:" + vnpayTranId;
                ViewBag.InnerTextvnp_Amount = "Số tiền thanh toán (VND):" + vnp_Amount + "Đ";
                ViewBag.InnerTextbankCode = "Ngân hàng thanh toán:" + bankCode;
                ViewBag.date = "ngày giao dịch" + parsedQuery.vnp_PayDate;
            
           



        }
        else
        {
            ViewBag.ErrorMessage = "Có lỗi xảy ra trong quá trình xử lý";
        }

        return View();


    }
    [HttpGet]
    public ActionResult CreatePaymentmomo()
    {
        string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
        string partnerCode = "MOMOBKUN20180529";
        string accessKey = "klm05TvNBzhg7h7j";
        string secretKey = "at67qH6mk8w5Y1nAyMoYKMWACiEi2bsa";
        string orderInfo = "Thanh toán qua MoMo";
        string amount = "10000";
        string orderId = DateTime.Now.Ticks.ToString();
        string redirectUrl = "https://webhook.site/b3088a6a-2d17-4f8d-a383-71389a6c600b";
        string ipnUrl = "https://webhook.site/b3088a6a-2d17-4f8d-a383-71389a6c600b";
        string extraData = "";
       


            partnerCode = partnerCode;
            accessKey = accessKey;
            secretKey = secretKey;
            orderId = orderId;
            orderInfo = orderInfo;
            amount = amount;
            ipnUrl = ipnUrl;
            redirectUrl = redirectUrl;
            extraData = extraData;

            string requestId = DateTime.Now.Ticks.ToString();
            string requestType = "payWithATM";
           
            // before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey + "&amount=" + amount + "&extraData=" + extraData + "&ipnUrl=" + ipnUrl + "&orderId=" + orderId + "&orderInfo=" + orderInfo + "&partnerCode=" + partnerCode + "&redirectUrl=" + redirectUrl + "&requestId=" + requestId + "&requestType=" + requestType;
            string signature = GetHmacSha256Hash(rawHash, secretKey);

            Dictionary<string, string> data = new Dictionary<string, string>
        {
            { "partnerCode", partnerCode },
            { "partnerName", "Test" },
            { "storeId", "MomoTestStore" },
            { "requestId", requestId },
            { "amount", amount },
            { "orderId", orderId },
            { "orderInfo", orderInfo },
            { "redirectUrl", redirectUrl },
            { "ipnUrl", ipnUrl },
            { "lang", "vi" },
            { "extraData", extraData },
            { "requestType", requestType },
            { "signature", signature }
        };

            string result = ExecPostRequest(endpoint, JsonConvert.SerializeObject(data));
            dynamic jsonResult = JsonConvert.DeserializeObject(result);
        string la = jsonResult.ToString();
        string filePath = @"D:\20paymentUrl.txt";

        // Ghi giá trị của paymentUrl vào tệp
        System.IO.File.WriteAllText(filePath, la);
        // Just an example, please handle the response accordingly
        return Redirect(jsonResult["payUrl"].ToString());

       

    }

}
    
public partial class sanphamthanhtoan
{
  public  Tuple<hienthisanpham, User> tuple { get; set; }

}



