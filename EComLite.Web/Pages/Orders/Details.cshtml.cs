using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EComLite.Web.Data;
using EComLite.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EComLite.Web.Pages.Orders
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public DetailsModel(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // ViewModel ------------------------------

        public OrderDetailView Order { get; set; } = default!;

        public class OrderDetailView
        {
            public Guid OrderId { get; set; }
            public string OrderNumber { get; set; } = string.Empty;
            public DateTime PlacedAt { get; set; }
            public decimal TotalAmount { get; set; }
            public string Status { get; set; } = string.Empty;
            public List<OrderItemView> Items { get; set; } = new();
        }

        public class OrderItemView
        {
            public string ProductName { get; set; } = string.Empty;
            public int Qty { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal LineTotal => Qty * UnitPrice;
        }

        // GET /Orders/Details/{id}
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var order = await _db.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .Where(o => o.OrderId == id && o.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            Order = new OrderDetailView
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                PlacedAt = order.PlacedAt,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Items = order.Items
                    .Select(i => new OrderItemView
                    {
                        ProductName = i.Product?.Name ?? $"Product #{i.ProductId}",
                        Qty = i.Qty,
                        UnitPrice = i.UnitPriceSnapshot
                    })
                    .ToList()
            };

            return Page();
        }
    }
}
