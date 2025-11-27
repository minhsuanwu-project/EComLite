using System;
using System.Threading.Tasks;
using EComLite.Web.Data;
using EComLite.Web.Models;
using EComLite.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EComLite.Web.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly CartService _cartService;

        public DetailsModel(ApplicationDbContext db, CartService cartService)
        {
            _db = db;
            _cartService = cartService;
        }

        public Product Product { get; set; } = null!;

        [BindProperty]
        public int Qty { get; set; } = 1;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null || product.IsArchived)
            {
                return NotFound();
            }

            Product = product;
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(Guid id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null || product.IsArchived)
            {
                return NotFound();
            }

            if (Qty <= 0) Qty = 1;

            _cartService.AddItem(new CartItem
            {
                ProductId = product.ProductId,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Qty = Qty
            });

            TempData["Message"] = "Item added to cart.";
            return RedirectToPage("/Cart/Index");
        }
    }
}
