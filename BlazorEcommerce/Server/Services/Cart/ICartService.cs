using Shared;

namespace Server.Services.Cart
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartProductResponse>>> GetCartProductsAsync(List<CartItem> cartItems);
        Task<ServiceResponse<List<CartProductResponse>>> StoreCartAsync(List<CartItem> cartItems);
        Task<ServiceResponse<int>> GetCartItemsCountAsync();
        Task<ServiceResponse<List<CartProductResponse>>> GetDbCartProductsAsync();
    }

}