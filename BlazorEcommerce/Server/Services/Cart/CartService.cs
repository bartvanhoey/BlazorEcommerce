using System.Security.Claims;
using Server.Data;
using Server.Services.Auth;
using Shared;

namespace Server.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly DatabaseContext _dbContext;
        private readonly IAuthService _authService;

        public CartService(DatabaseContext dbContext, IAuthService authService)
        {
            _authService = authService;
            _dbContext = dbContext;
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> GetCartProductsAsync(List<CartItem> cartItems)
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

        public async Task<ServiceResponse<int>> GetCartItemsCountAsync()
        {
            var count = (await _dbContext.CartItems.Where(ci => ci.UserId == _authService.GetUserId()).ToListAsync()).Count;
            return new ServiceResponse<int> { Data = count };
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> GetDbCartProductsAsync(int? userId = null)
        {
            userId ??= _authService.GetUserId();
            return await GetCartProductsAsync(await _dbContext.CartItems.Where(x => x.UserId == userId).ToListAsync());
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> StoreCartAsync(List<CartItem> cartItems)
        {
            var userId = _authService.GetUserId();
            cartItems.ForEach(ci => ci.UserId = userId);
            _dbContext.CartItems.AddRange(cartItems);
            await _dbContext.SaveChangesAsync();
            return await GetDbCartProductsAsync();
        }

        

        public async Task<ServiceResponse<bool>> AddToCartAsync(CartItem cartItem)
        {
            cartItem.UserId = _authService.GetUserId();
            var sameItem = await _dbContext.CartItems.FirstOrDefaultAsync(c => c.ProductId == cartItem.ProductId
                        && c.ProductTypeId == cartItem.ProductTypeId
                        && c.UserId == cartItem.UserId);

            if (sameItem == null)
            {
                _dbContext.CartItems.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }

            await _dbContext.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> UpdateQuantityAsync(CartItem cartItem)
        {
            var dbCartItem = await _dbContext.CartItems.FirstOrDefaultAsync(c => c.ProductId == cartItem.ProductId
                        && c.ProductTypeId == cartItem.ProductTypeId
                        && c.UserId == _authService.GetUserId());

            if (dbCartItem == null) return new ServiceResponse<bool> { Data = false, Message = "Cart item does not exist", Success = false };
            dbCartItem.Quantity = cartItem.Quantity;
            await _dbContext.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> RemoveItemFromCartAsync(int productId, int productTypeId)
        {
            var dbCartItem = await _dbContext.CartItems.FirstOrDefaultAsync(c => c.ProductId == productId
                                    && c.ProductTypeId == productTypeId && c.UserId == _authService.GetUserId());

            if (dbCartItem == null) return new ServiceResponse<bool> { Data = false, Message = "Cart item does not exist", Success = false };

            _dbContext.CartItems.Remove(dbCartItem);
            await _dbContext.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };

        }
    }
}