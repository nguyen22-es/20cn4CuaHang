using CuaHangCongNghe.viewModel;
using Microsoft.AspNetCore.Mvc;

namespace CuaHangCongNghe.Service
{
    public interface IPaymentService 
    {
       string PaymentCall(UserOrderViewModel userOrderView);

      List< string> CheckPaymentStatus(string IdUser, string urlResponse);

    }
}
