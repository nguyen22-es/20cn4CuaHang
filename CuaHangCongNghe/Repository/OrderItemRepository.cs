
using CuaHangCongNghe.Models.Shop;
using System;

namespace CuaHangCongNghe
{
    public interface OrderItemRepository
    {
        Order GetOrder(int IdOrder);
        List<Order> GetAll();
        Order Create(string userId, Product product,int quantity);
        OrderItem AddItem(int id, OrderItem OrderItem);
        void Update(OrderItem  OrderItem);
        void DeleteItem(int OrderItem);
        void DeleteOder(int orderId);
        Order UpdateOrder(int Id, Product product, int quantity);
        List<Order> TryGetByOrderUserId(string UserId);
        List<Order> GetAllOrderPay();
        List<OrderItem> getAllItem(int idItem);

        Order getOrderPay(string  userId);

        void ChangeStatus(int id, int status);
    }
}