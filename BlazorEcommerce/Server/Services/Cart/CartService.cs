using Microsoft.AspNetCore.Http.Features;
using Server.Data;
using Shared;

namespace Server.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly DatabaseContext _dbContext;

        public CartService(DatabaseContext dbContext) => _dbContext = dbContext;

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
                            .Where(x => x.ProductId == item.ProductId && x.ProductTypeId == item.ProductTypeId).FirstOrDefaultAsync();
                if (productVariant == null) continue;

                var cartProduct = new CartProductResponse()
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    ImageUrl = product.ImageUrl,
                    Price = productVariant.Price,
                    ProductType = productVariant?.ProductType?.Name ?? string.Empty,
                    ProductTypeId = productVariant?.ProductTypeId ?? 0
                };
                result.Data.Add(cartProduct);
            };
            return result;
        }
    }
}