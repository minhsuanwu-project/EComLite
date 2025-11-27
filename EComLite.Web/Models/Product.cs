using System;
using System.ComponentModel.DataAnnotations;

namespace EComLite.Web.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }

        [Required, StringLength(64)]
        public string Sku { get; set; } = string.Empty;

        [Required, StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Range(0, 9999999)]
        public decimal Price { get; set; }

        [StringLength(3)]
        public string Currency { get; set; } = "USD";

        [Range(0, int.MaxValue)]
        public int StockQty { get; set; }

        public bool IsArchived { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
