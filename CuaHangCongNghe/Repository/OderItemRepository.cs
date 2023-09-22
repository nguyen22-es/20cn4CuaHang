
using CuaHangCongNghe.Models.Shop;
using System;

namespace CuaHangCongNghe
{
    public interface OderItemRepository
    {
        List<Order> GetAll();
        Order Create(string userId, Product product,int quantity);
        Orderitem AddItem(int id, Orderitem orderitem);
        void Update(Order order);
        void DeleteItem(int orderitem);
        void DeleteOder(int orderId);
        Order AddProduct(int Id, Product product, int quantity);

        List<Order> TryGetByOrderUserId(string UserId);

        List<Orderitem> getAllItem(int idItem);

        Order getOrderPay(string  userId);

        void ChangeStatus(int id, int status);
    }
}