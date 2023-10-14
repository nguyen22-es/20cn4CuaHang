using CuaHangCongNghe.Models;
using CuaHangCongNghe.Repository;
using CuaHangCongNghe.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace CuaHangCongNghe.Pages.Shared.Components.BasketComponent // cho phép tạo và sử dụng các thành phần giao diện có thể sử dụng lại trên nhiều thành phần khác nhau
{
    public class Cart : ViewComponent
    {
        private readonly OrderItemService  oderItemService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public Cart(OrderItemService basketService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            oderItemService = basketService;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userName) // sẽ được gọi khi sử dụng trong 1 trang Razor 
        {
            var itemsCount = GetBasketViewModel().FullAmount;
            return View(itemsCount);
        }

        private OrderViewModel GetBasketViewModel()
        {
            if (signInManager.IsSignedIn(HttpContext.User))
            {
                var userId = userManager.GetUserId(HttpContext.User);
                return oderItemService.GetOrderPay(userId);
            }
            string anonymousId = GetBasketIdFromCookie();
            if (anonymousId == null)
                return new OrderViewModel();
            return oderItemService.GetOrderPay(anonymousId);
        }

        private string GetBasketIdFromCookie()
        {
            if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
            {
                return Request.Cookies[Constants.BASKET_COOKIENAME];
            }
            return null;
        }
    }
}
// hiển thị số lượng sản phẩm trong giỏ hàng của người dùng trên giao diện người dùng và quản lý thông tin về giỏ hàng dựa trên người dùng hiện tại hoặc dựa trên cookie nếu người dùng chưa đăng nhập