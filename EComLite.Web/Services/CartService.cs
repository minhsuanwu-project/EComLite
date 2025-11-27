using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using EComLite.Web.Models;
using Microsoft.AspNetCore.Http;

namespace EComLite.Web.Services
{
    public class CartService
    {
        private const string CartSessionKey = "CartItems";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext!.Session;

        public List<CartItem> GetCart()
        {
            var json = Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(json))
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }

        public void SaveCart(List<CartItem> items)
        {
            var json = JsonSerializer.Serialize(items);
            Session.SetString(CartSessionKey, json);
        }

        public void AddItem(CartItem item)
        {
            var cart = GetCart();
            var existing = cart.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (existing == null)
            {
                cart.Add(item);
            }
            else
            {
                existing.Qty += item.Qty;
            }

            SaveCart(cart);
        }

        public void Clear()
        {
            Session.Remove(CartSessionKey);
        }

        public void Remove(Guid productId)  
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
        }

    }
}

