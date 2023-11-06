
using CuaHangCongNghe.Models;
using CuaHangCongNghe.Models.Shop;


namespace CuaHangCongNghe.Models
{
    public static class MappingExtensions
    {     

        public static List<OrderViewModel> ToOrdersViewModel(this List<Order> orders)
        {
            var items = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                var item = order.ToOrderViewModel();
               items.Add(item);
            }

           
            return items;
        }
        public static OrderViewModel ToOrderViewModel(this Order Order)
        {
            return new OrderViewModel
            {
                Id = Order.OrderId,
                OrderDate = Order.OrderDate,
                Status = Order.Status,
                UserId = Order.UserId,
                NameStatus = Order.StatusNavigation.StatusName,
                ItemViewModels = Order.OrderItems.ToList().ToOrderItemsViewModel()
            };
        }

        public static List<OrderItemViewModel> ToOrderItemsViewModel(this List<OrderItem> OrderItems)
        {
            var items = new List<OrderItemViewModel>();
            foreach (var orderItem in OrderItems)
            {
                var item = orderItem.ToOrderItemViewModel();
                items.Add(item);
            }
            return items;
        }

        public static OrderItemViewModel ToOrderItemViewModel(this OrderItem OrderItem)
        {
            return new OrderItemViewModel
            {
                Id = OrderItem.OrderItemsId,
                quantity = OrderItem.Quantity,
                Product = OrderItem.Product.ToProductViewModel()
            };
        }

        public static ProductViewModel ToProductViewModel(this Product product)
        {
            return new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Stockquantity = product.Stockquantity,
                
                
            };
        }

        public static Product ToProduct(this ProductViewModel productViewModel)
        {
            var path = productViewModel.ImageUrl;
            if (productViewModel.File != null)
            {
                path = "/images/" + productViewModel.File.FileName;
            }
            return new Product()
            {
                Id = productViewModel.Id,
                Name = productViewModel.Name,
                Description = productViewModel.Description,
                Price = productViewModel.Price,
                Stockquantity = productViewModel.Stockquantity,
                ImageUrl = path
            };
        }

        public static UserViewModel toUserViewModel(this User user)
        {
            return new UserViewModel
            {
                Id = user.UserId,
                PhoneUser = user.PhoneUser,
                AddressUser = user.AddressUser,
                NameUser = user.NameUser,
                EmailUser = user.EmailUser,
                RegistrationDate = user.RegistrationDate,
                  
               
            };
        }

        public static User ToUserInfo(this UserViewModel userViewModel)
        {
            return new User
            {
                UserId = userViewModel.Id,
                PhoneUser = userViewModel.PhoneUser,
                AddressUser = userViewModel.AddressUser,
                NameUser = userViewModel.NameUser,
                EmailUser = userViewModel.EmailUser,
                        
                RegistrationDate = userViewModel.RegistrationDate,
             
              

            };
        }
    }
}
