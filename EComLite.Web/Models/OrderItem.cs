using System;

namespace EComLite.Web.Models
{
    public class OrderItem
    {
        public long OrderItemId { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Qty { get; set; }

        public decimal UnitPriceSnapshot { get; set; }

        public decimal LineTotal => Qty * UnitPriceSnapshot;
    }
}
