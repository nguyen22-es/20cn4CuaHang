using CuaHangCongNghe.Models;
using CuaHangCongNghe.Repository;
using CuaHangCongNghe.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace CuaHangCongNghe.Pages.Shared.Components.BasketComponent
{
    public class Cart : ViewComponent
    {
        private readonly oderItemService  oderItemService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public Cart(oderItemService basketService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            oderItemService = basketService;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userName)
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
