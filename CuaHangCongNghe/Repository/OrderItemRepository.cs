
using CuaHangCongNghe.Models.Shop;
using System;
using static NuGet.Packaging.PackagingConstants;

namespace CuaHangCongNghe
{
    public interface OrderItemRepository
    {
        Order GetOrder(int IdOrder);
        List<Order> GetAll();
        Order Create(string userId, Product product,int quantity);
        void CreateItem(int Id, Product product);
        void Update(OrderItem  OrderItem);
        void DeleteItem(int OrderItem);
        void DeleteOder(int orderId);
        void UpdateOrder(Order order, Product product, int quantity);
        List<Order> TryGetByOrderUserId(string UserId);
        List<Order> GetAllOrderPay();
        List<OrderItem> getAllItem(int idItem);

        Order getOrderPay(string  userId);

        void ChangeStatus(int id, int status);
    }
}