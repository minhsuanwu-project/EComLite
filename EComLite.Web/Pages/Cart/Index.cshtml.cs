using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EComLite.Web.Data;
using EComLite.Web.Models;
using EComLite.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EComLite.Web.Pages.Cart
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(CartService cartService, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _cartService = cartService;
            _db = db;
            _userManager = userManager;
        }

        public List<CartItem> Items { get; set; } = new();

        public decimal Total => Items.Sum(i => i.Qty * i.UnitPrice);

        public void OnGet()
        {
            Items = _cartService.GetCart();
        }

        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            Items = _cartService.GetCart();
            if (!Items.Any())
            {
                TempData["Message"] = "Cart is empty.";
                return RedirectToPage();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }
            var placedAt = DateTime.UtcNow;
            var orderId = Guid.NewGuid();

            var order = new Order
            {
                OrderId = orderId,
                OrderNumber = GenerateOrderNumber(orderId, placedAt),
                UserId = user.Id,
                TotalAmount = Total,
                Currency = "USD",
                Status = "Placed",
                PlacedAt = System.DateTime.UtcNow
            };

            foreach (var item in Items)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Qty = item.Qty,
                    UnitPriceSnapshot = item.UnitPrice
                });
            }

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            _cartService.Clear();

            TempData["Message"] = $"Order {order.OrderId} placed successfully.";
            return RedirectToPage("/Orders/Index");
        }
        public IActionResult OnPostClear()
        {
            _cartService.Clear();
            return RedirectToPage();
        }
        public IActionResult OnPostRemove(Guid productId)
        {
            _cartService.Remove(productId);
            return RedirectToPage();
        }
        private string GenerateOrderNumber(Guid orderId, DateTime placedAtUtc)
        {            
            var randomPart = orderId.ToString("N")[..4].ToUpper(); // Get first 4-digit from OrderId 
            return $"ORD-{placedAtUtc:yyyyMMdd}-{randomPart}";
        }

    }
}

