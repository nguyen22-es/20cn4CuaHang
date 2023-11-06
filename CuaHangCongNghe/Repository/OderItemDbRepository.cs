using CuaHangCongNghe.Models;
using CuaHangCongNghe.Models.Shop;

using Microsoft.EntityFrameworkCore;


namespace CuaHangCongNghe.Repository
{
    public class OderDbRepository : OrderItemRepository
    {
        private readonly storeContext storeContext;
        public OderDbRepository(storeContext storeContext)
        {
            this.storeContext = storeContext;
        }
        public void CreateItem(int Id, Product product)
        {
            var orderItem = new OrderItem()
            {
                OrderId = Id,
                ProductId = product.Id,
                Quantity = 1,
            };
            
            storeContext.Orderitems.Add(orderItem);
            storeContext.SaveChanges();


        }

        public void UpdateOrder(Order order, Product product,int quantity)
        {
           
            var existingSameItem = order.OrderItems.FirstOrDefault(x => x.ProductId == product.Id);
            if (existingSameItem != null)
            {
                existingSameItem.Quantity = quantity;
                Update(existingSameItem);
              
            }
            else
            {
                order.OrderItems.Add(new OrderItem { OrderId = order.OrderId, ProductId = product.Id, Quantity = quantity });
                storeContext.SaveChanges();
            }
            
       
        }

        public void ChangeStatus(int id, int status)
        {
            var order = storeContext.Orders.FirstOrDefault(x => x.OrderId == id);
            order.Status = status;
            storeContext.SaveChanges();
        }

        public Order Create(string userId, Product product, int quantity)
        {
            var order = new Order 
            { 
                UserId = userId,
            
                OrderDate = DateTime.Now,
                Status = 0,

            };
            storeContext.Orders.Add(order);
            order.OrderItems.Add(new OrderItem { OrderId = order.OrderId, ProductId = product.Id, Quantity = quantity });
            storeContext.SaveChanges();
            return order;
        }

        public void DeleteItem(int idItemOrder)
        {
            var order = storeContext.Orderitems.FirstOrDefault(x => x.OrderItemsId == idItemOrder);
            storeContext.Orderitems.Remove(order);
            storeContext.SaveChanges();
        }

        public void DeleteOder(int  orserId)
        {
            var order = storeContext.Orders.FirstOrDefault(x => x.OrderId == orserId);
            storeContext.SaveChanges();
        }

      

        public List<Order> GetAll()
        {        
            return storeContext.Orders.AsNoTracking().Include(status => status.StatusNavigation)
        .Include(order => order.OrderItems)
        .ThenInclude(orderItem => orderItem.Product)
        .ToList();

        }

        public List<OrderItem> getAllItem(int orderId)
        {
            return storeContext.Orderitems
        .Where(OrderItem => OrderItem.OrderId == orderId).Include(c => c.Product)   
        .ToList();
        }

        public Order getOrderPay(string userId)
        {

            var Orders = GetAll();
           
            if (Orders != null)
            {
                foreach (var order in Orders)
                {
                    if (order.UserId == userId && order.Status==0)
                    {
                        return order;
                    }
                }
               
            }

            return null;



           // return  storeContext.Orders.Include(o => o.OrderItems).ThenInclude(p => p.Product).FirstOrDefault(n => n.UserId == userId && n.Status == 0);// lấy status chưa thanh toán
        }
        

        public List<Order> TryGetByOrderUserId(string UserId)
        {
          
           var listOder = GetAll();
            var allOrderUser = new List<Order>();
            if (listOder != null)
            {
                foreach (Order oder in listOder)
                {
                    if (oder.UserId == UserId)
                    {
                        allOrderUser.Add(oder);
                    }
                }
                return allOrderUser;
            }
            
                return null;
            
            
        }


        public void Update(OrderItem existingOrder)
        {
            var order = storeContext.Orderitems.FirstOrDefault(x => x.OrderItemsId == existingOrder.OrderItemsId);
            order.Quantity = existingOrder.Quantity;
            storeContext.SaveChanges();
        }



        public Order GetOrder(int IdOrder)
        {
            var Orders = GetAll();
           
            if (Orders != null)
            {
                foreach (var order in Orders)
                {
                    if (order.OrderId == IdOrder)
                    {
                        return order;
                    }
                }
               
            }

            return null;
        }

        public List<Order> GetAllOrderPay()
        {
            var listOder = GetAll();
            var allOrderUser = new List<Order>();
            if (listOder != null)
            {
                foreach (Order oder in listOder)
                {
                    if (oder.Status == 2)
                    {
                        allOrderUser.Add(oder);
                    }
                }
                return allOrderUser;
            }

            return null;
        }
    }
}
