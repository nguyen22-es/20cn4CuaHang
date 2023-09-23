
using CuaHangCongNghe.Models;
using CuaHangCongNghe.Models.Shop;
using Shop.Models;


namespace CuaHangCongNghe.Service

{
    public class oderItemService
    {
        private readonly OderItemRepository oderItemRepository;

        public oderItemService(OderItemRepository oderItemRepository)
        {
            this.oderItemRepository = oderItemRepository;
        }

        public OrderViewModel AddProductToItems(ProductViewModel productViewModel, string userId, int amount)
        {
            var orders = oderItemRepository.getOrderPay(userId);
            var product = productViewModel.ToProduct();

            Order order;

            
            if (orders == null)
            {
               
                order = oderItemRepository.Create(userId, product,amount);
            }
            else
            {
                order = oderItemRepository.AddProduct(orders.OrderId, product,amount);
            }

            var OrderViewModel = new OrderViewModel
            {
                Status = order.Status,
                Id = order.OrderId,
                UserId = order.UserId,
                ItemViewModels = order.Orderitems.ToList().ToOrderItemsViewModel()
            };
            return OrderViewModel;
        }

        public List<OrderViewModel> GetCurrentAllOrder()
        {

            var existingOrder = oderItemRepository.GetAll();

            if (existingOrder != null)
            {
                return existingOrder.ToOrdersViewModel();



            }

            return null;
        }

        public List<OrderViewModel> GetCurrentAllOrder(string userId)
        {
           
            var existingOrder = oderItemRepository.TryGetByOrderUserId(userId);
           
            if (existingOrder != null)
            {
                return existingOrder.ToOrdersViewModel();
                 


            }

            return null;
        }

        public List<OrderItemViewModel> GetOrderItem(int OrderId)
        {
          return  oderItemRepository.getAllItem(OrderId).ToOrderItemsViewModel();

        }
    

        public void DeleteItem(string userId, int orderItem)
        {
            var existingOrder = oderItemRepository.getOrderPay(userId);
            var OrderItem = existingOrder.Orderitems.FirstOrDefault(x => x.OrderItemsId == orderItem);
            oderItemRepository.DeleteItem(orderItem);
        }

        public void UpdateAmount(string userId, int cartItemId, int amount)
        {
            var existingOrder = oderItemRepository.getOrderPay(userId);
            if (existingOrder != null)
            {
                var OrderItem = existingOrder.Orderitems.FirstOrDefault(x => x.OrderItemsId == cartItemId);
                OrderItem.Quantity += amount;
            }


            oderItemRepository.Update(existingOrder);
        }

        

        public void DeleteOrder(int OrderId)
        {       
                oderItemRepository.DeleteOder(OrderId);
        }

        public void ChangeStatus(int orderId, int status)
        {
            oderItemRepository.ChangeStatus(orderId, status);
        }
    }
}