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

        public OrderService(DatabaseContext context, ICartService cartService, IAuthService authService)
        {
            _db = context;
            _cartService = cartService;
            _authService = authService;
        }

        public async Task<ServiceResponse<OrderDetailsResponse>> GetOrderDetailsAsync(int orderId)
        {
            var response = new ServiceResponse<OrderDetailsResponse>();

            var order = await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ProductType)
                .Where(o => o.UserId == _authService.GetUserId() && o.Id == orderId)
                .OrderByDescending(o => o.OrderDate).FirstOrDefaultAsync();

            if (order == null)
            {
                response.Success = false;
                response.Message = "Order not found";
                return response;
            }

            var orderDetailsResponse = new OrderDetailsResponse()
            {
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Products = new List<OrderDetailsProductResponse>()
            };

            order.OrderItems.ForEach(oi
                => orderDetailsResponse.Products.Add(new OrderDetailsProductResponse
                {
                    ProductId = oi.ProductId,
                    ImageUrl = oi.Product.ImageUrl,
                    ProductType = oi.ProductType.Name,
                    Quantity = oi.Quantity,
                    Title = oi.Product.Title,
                    TotalPrice = oi.TotalPrice
                }));

            response.Data = orderDetailsResponse;
            return response;
        }

        public async Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrdersAsync()
        {
            var response = new ServiceResponse<List<OrderOverviewResponse>>();
            var orders = await _db.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .Where(o => o.UserId == _authService.GetUserId())
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var orderResponse = new List<OrderOverviewResponse>();
            orders.ForEach(o => orderResponse.Add(new OrderOverviewResponse()
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Product = o.OrderItems.Count > 1 ? $"{o.OrderItems.First().Product.Title} and {o.OrderItems.Count} more... " : o.OrderItems.First().Product.Title,
                ProductImageUrl = o.OrderItems.First().Product.ImageUrl

            }
            ));

            response.Data = orderResponse;
            return response;
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