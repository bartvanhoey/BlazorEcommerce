using System.Security.Claims;
using Server.Data;
using Server.Services.Auth;
using Server.Services.Cart;
using Shared;

namespace Server.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly DatabaseContext _db;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;

        public OrderService(DatabaseContext context, ICartService cartService, IAuthService authService )
        {
            _db = context;
            _cartService = cartService;
            _authService = authService;
        }

        public async Task<ServiceResponse<bool>> PlaceOrderAsync()
        {
            var products = (await _cartService.GetDbCartProductsAsync()).Data;
            if (products == null || products.Count == 0) return new ServiceResponse<bool>() { Data = true };

            decimal totalPrice = 0;
            products.ForEach(p => totalPrice += p.Price * p.Quantity);

            var orderItems = new List<OrderItem>();
            products.ForEach(p => orderItems.Add(new OrderItem
            {
                ProductId = p.ProductId,
                ProductTypeId = p.ProductTypeId,
                Quantity = p.Quantity,
                TotalPrice = p.Price * p.Quantity
            }));

            var order = new Order()
            {
                UserId = _authService.GetUserId(),
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            _db.Orders.Add(order);
            _db.CartItems.RemoveRange(_db.CartItems.Where(ci => ci.UserId == _authService.GetUserId()));

            await _db.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        
    }
}