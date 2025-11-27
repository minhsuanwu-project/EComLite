using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EComLite.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EComLite.Web.Pages.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public List<OrderView> Orders { get; set; } = new();

        public class OrderView
        {
            public Guid OrderId { get; set; }
            public string OrderNumber { get; set; } = string.Empty;
            public DateTime PlacedAt { get; set; }
            public decimal TotalAmount { get; set; }
            public string Status { get; set; } = string.Empty;
            public string ProductName { get; set; } = string.Empty;
            public int TotalQty { get; set; }
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return;
            // 1. Get basic columns + Qty
            var orderRows = await _db.Orders
            .Where(o => o.UserId == user.Id)
            .OrderByDescending(o => o.PlacedAt)
            .Select(o => new OrderView
            {

                OrderId  = o.OrderId,
                OrderNumber = o.OrderNumber,
                PlacedAt = o.PlacedAt,               
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                TotalQty = o.Items.Sum(i => i.Qty)
            })
            .ToListAsync();

            Orders = orderRows
            .Select(o => new OrderView
            {
                OrderId = o.OrderId,

                // Order number Ex:ORD-20251120-AB12
                OrderNumber = $"ORD-{o.PlacedAt:yyyyMMdd}-{o.OrderId.ToString("N").Substring(0, 4).ToUpper()}",

                PlacedAt = o.PlacedAt,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                TotalQty = o.TotalQty
            })
            .ToList();
            }
        }
}

