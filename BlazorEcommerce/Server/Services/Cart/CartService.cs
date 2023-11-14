using System.Security.Authentication;
using System.Security.Claims;
using Server.Data;
using Shared;

namespace Server.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly DatabaseContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public CartService(DatabaseContext dbContext, IHttpContextAccessor accessor)
        {
            _dbContext = dbContext;
            _accessor = accessor;
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> GetCartAsync(List<CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartProductResponse>>
            {
                Data = new List<CartProductResponse>()
            };

            foreach (var item in cartItems)
            {
                var product = await _dbContext.Products.Where(x => x.Id == item.ProductId).FirstOrDefaultAsync();
                if (product == null) continue;

                var productVariant = await _dbContext.ProductVariants
                            .Where(x => x.ProductId == item.ProductId && x.ProductTypeId == item.ProductTypeId).Include(p => p.ProductType).FirstOrDefaultAsync();
                if (productVariant == null) continue;

                var cartProduct = new CartProductResponse()
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    ImageUrl = product.ImageUrl,
                    Price = productVariant.Price,
                    ProductType = productVariant?.ProductType?.Name ?? string.Empty,
                    ProductTypeId = productVariant?.ProductTypeId ?? 0,
                    Quantity = item.Quantity
                };
                result.Data.Add(cartProduct);
            };
            return result;
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
            var count = (await _dbContext.CartItems.Where(ci => ci.UserId == GetUserId()).ToListAsync()).Count;
            return new ServiceResponse<int> { Data = count };
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> StoreCartAsync(List<CartItem> cartItems)
        {
            var userId = GetUserId();
            cartItems.ForEach(ci => ci.UserId = userId);
            _dbContext.CartItems.AddRange(cartItems);
            await _dbContext.SaveChangesAsync();
            return await GetCartAsync(
                await _dbContext.CartItems.Where(ci => ci.UserId == userId).ToListAsync());
        }

        private int GetUserId()
            => int.Parse(_accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "-1");

    }
}