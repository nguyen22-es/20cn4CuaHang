
using CuaHangCongNghe.Models;
using CuaHangCongNghe.Models.Shop;
using CuaHangCongNghe.Models;


namespace CuaHangCongNghe.Service

{
    public class OrderItemService :IOrderItemService
    {
        private readonly OrderItemRepository OrderItemRepository;

        public OrderItemService(OrderItemRepository OrderItemRepository)
        {
            this.OrderItemRepository = OrderItemRepository;
        }

       /* public OrderViewModel AddProductToItems(ProductViewModel productViewModel, string userId, int amount)
        {
            var orders = OrderItemRepository.getOrderPay(userId);
            var product = productViewModel.ToProduct();
          
            Order order;

            
            if (orders == null)
            {
               
                order = OrderItemRepository.Create(userId, product,amount);
            }
            else
            {
                order = OrderItemRepository.UpdateOrder(orders.OrderId, product,amount);
            }

            var OrderViewModel = new OrderViewModel
            {
                Status = 0,
                Id = order.OrderId,
                UserId = order.UserId,
                ItemViewModels = order.OrderItems.ToList().ToOrderItemsViewModel()
            };
            return OrderViewModel;
        }*/
        public void AddCart(ProductViewModel productViewModel, string userId)
        {
            var orders = OrderItemRepository.getOrderPay(userId);
            var product = productViewModel.ToProduct();

          


            if (orders == null)
            {

              OrderItemRepository.Create(userId, product, 1);
            }
            else
            {
                var item = orders.OrderItems.FirstOrDefault(c => c.ProductId == product.Id);
                if(item != null)
                {
                 var amount =    item.Quantity +1;
                    OrderItemRepository.UpdateOrder(orders, product, amount);
                }
                else
                {


                    OrderItemRepository.CreateItem(orders.OrderId, product);
                }
            
            }


        }

        public List<OrderViewModel> GetCurrentAllOrder()
        {

            var existingOrder = OrderItemRepository.GetAll();

            if (existingOrder != null)
            {
                return existingOrder.ToOrdersViewModel();
            }

            return null;
        }

        public List<OrderViewModel> GetCurrentAllOrderUser(string userId)
        {
           
            var existingOrder = OrderItemRepository.TryGetByOrderUserId(userId);
           
            if (existingOrder != null)
            {
                return existingOrder.ToOrdersViewModel();
            }

            return null;
        }

        public  OrderViewModel GetOrder(int  IdOrder)
        {
            var Order = OrderItemRepository.GetOrder(IdOrder).ToOrderViewModel();

            return Order;
        }
        public List<OrderViewModel> GetAllOrdernPay()
        {
            var Order = OrderItemRepository.GetAllOrderPay().ToOrdersViewModel();

            return Order;
        }


        public OrderViewModel GetOrderPay(string userId)
        {
            var existingOrder = OrderItemRepository.getOrderPay(userId);

            if (existingOrder != null)
            {
                return new OrderViewModel()
                {
                    Id = existingOrder.OrderId,
                    ItemViewModels = existingOrder.OrderItems.ToList().ToOrderItemsViewModel()
                };
            }

            return new OrderViewModel()
            {
                ItemViewModels = new List<OrderItemViewModel>()
            };

        }
    

        public List<OrderItemViewModel> GetOrderItem(int OrderId)
        {
           
           var items = OrderItemRepository.getAllItem(OrderId);


            return items?.ToOrderItemsViewModel();

        }
    

        public void DeleteItem(string userId, int orderItem)
        {
            var existingOrder = OrderItemRepository.getOrderPay(userId);
            var OrderItem = existingOrder.OrderItems.FirstOrDefault(x => x.OrderItemsId == orderItem);
            OrderItemRepository.DeleteItem(orderItem);
        }

          public void UpdateQuantity(string userId, int cartItemId, int quantity)
        {
    
        var existingOrder = OrderItemRepository.getOrderPay(userId);

  
        if (existingOrder == null)
        {
            return;
        }

    
        var cartItem = existingOrder.OrderItems.FirstOrDefault(x => x.OrderItemsId == cartItemId);

   
        if (cartItem == null)
        {
            return;
        }

   
        cartItem.Quantity = quantity;

        // Update the order.
        OrderItemRepository.Update(cartItem);
    }

        

        public void DeleteOrder(int OrderId)
        {       
                OrderItemRepository.DeleteOder(OrderId);
        }

        public void ChangeStatus(int orderId, int status)
        {
            OrderItemRepository.ChangeStatus(orderId, status);
        }
    }
}