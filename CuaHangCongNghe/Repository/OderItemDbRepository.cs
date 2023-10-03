using CuaHangCongNghe.Models;
using CuaHangCongNghe.Models.Shop;

using Microsoft.EntityFrameworkCore;


namespace CuaHangCongNghe.Repository
{
    public class OderDbRepository : OderItemRepository
    {
        private readonly storeContext storeContext;
        public OderDbRepository(storeContext storeContext)
        {
            this.storeContext = storeContext;
        }
        public Orderitem AddItem(int id, Orderitem orderitem)
        {
            throw new NotImplementedException();
        }

        public Order AddProduct(int Id, Product product,int quantity)
        {
            var order = storeContext.Orders.FirstOrDefault(x => x.OrderId == Id);
            var existingSameProduct = order.Orderitems.FirstOrDefault(x => x.ProductId == product.Id);
            if (existingSameProduct != null)
            {
                existingSameProduct.Quantity = quantity;
            }
            else
            {
                order.Orderitems.Add(new Orderitem { OrderId = order.OrderId, ProductId = product.Id, Quantity = quantity });
            }
            storeContext.SaveChanges();
            return order;
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
            order.Orderitems.Add(new Orderitem { OrderId = order.OrderId, ProductId = product.Id, Quantity = quantity });
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
            return storeContext.Orders.AsNoTracking()
        .Include(order => order.Orderitems)
        .ThenInclude(orderItem => orderItem.Product)
        .ToList();

        }

        public List<Orderitem> getAllItem(int orderId)
        {
            return storeContext.Orderitems
        .Where(Orderitem => Orderitem.OrderId == orderId)    
        .ToList();
        }

        public Order getOrderPay(string userId)
        {
            return  storeContext.Orders.Include(o => o.Orderitems).ThenInclude(p => p.Product).FirstOrDefault(n => n.UserId == userId && n.Status == 0);// lấy order chưa thanh toán
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


        public void Update(Orderitem existingOrder)
        {
            var order = storeContext.Orderitems.FirstOrDefault(x => x.OrderItemsId == existingOrder.OrderItemsId);
            order = existingOrder;
            storeContext.SaveChanges();
        }



        public Order GetOrder(int IdOrder)
        {
            return storeContext.Orders.AsNoTracking().Include(o => o.Orderitems).ThenInclude(p => p.Product).FirstOrDefault(x => x.OrderId == IdOrder);
        }
    }
}
