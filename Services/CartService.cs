using Microsoft.AspNetCore.Http;
using Ecom_Demo.Extensions;
using Ecom_Demo.Models;
using System;

namespace Ecom_Demo.Services
{
    /// <summary>
    /// Access to the session-backed shopping cart.
    ///
    /// Previously every controller that touched the cart repeated the same
    /// three lines of session code (and its own copy of the session key),
    /// which is exactly how keys drift and items end up in two invisible
    /// carts. This is the single place that reads, writes, and clears the
    /// cart so the checkout, cart, and customization flows all share one.
    /// </summary>
    public class CartService : ICartService
    {
        private const string CartSessionKey = "ShoppingCart";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session =>
            _httpContextAccessor.HttpContext?.Session
            ?? throw new InvalidOperationException("HTTP session is not available in this context.");

        public ShoppingCart GetCart()
        {
            var cart = Session.GetObjectFromJson<ShoppingCart>(CartSessionKey);
            if (cart == null)
            {
                cart = new ShoppingCart();
                SaveCart(cart);
            }
            return cart;
        }

        public void SaveCart(ShoppingCart cart)
        {
            Session.SetObjectAsJson(CartSessionKey, cart);
        }

        public void Clear()
        {
            Session.Remove(CartSessionKey);
        }

        public int GenerateCartItemId()
        {
            return Math.Abs(Guid.NewGuid().GetHashCode());
        }
    }
}