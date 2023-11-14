namespace Client.Services.Cart
{
    public interface ICartService
    {
        Task AddToCartAsync(CartItem cartItem);
        Task<List<CartItem>> GetCartItemsAsync();
        Task<List<CartProductResponse>> GetCartProductsAsync();
        public event Action OnChange;
        Task DeleteProductFromCartAsync(int id, int productTypeId);
        Task UpdateQuantityAsync(CartProductResponse cartProductResponse);

    Task StoreCartItemsAsync(bool emptyLocalCart);


    }
}