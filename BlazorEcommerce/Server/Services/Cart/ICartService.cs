using Shared;

namespace Server.Services.Cart
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartProductResponse>>> GetCartAsync(List<CartItem> cartItems);
        Task<ServiceResponse<List<CartProductResponse>>> StoreCartAsync(List<CartItem> cartItems, int userId);
    }

}