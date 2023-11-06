using CuaHangCongNghe.Models;

namespace CuaHangCongNghe.Service
{
    public interface IOrderItemService
    {
        void AddCart(ProductViewModel productViewModel, string userId);
        List<OrderViewModel> GetCurrentAllOrder();
        List<OrderViewModel> GetCurrentAllOrderUser(string userId);
        OrderViewModel GetOrder(int IdOrder);
        List<OrderViewModel> GetAllOrdernPay();
        List<OrderItemViewModel> GetOrderItem(int OrderId);
        void DeleteItem(string userId, int orderItem);
        void DeleteOrder(int OrderId);
        void ChangeStatus(int orderId, int status);
        OrderViewModel GetOrderPay(string userId);
        void UpdateQuantity(string userId, int cartItemId, int quantity);



    }
}
