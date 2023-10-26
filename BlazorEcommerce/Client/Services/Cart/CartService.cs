

using Blazored.LocalStorage;

namespace Client.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        public CartService(ILocalStorageService localStorageService) => _localStorage = localStorageService;

        public async Task AddToCartAsync(CartItem cartItem) 
            => (await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>()).Add(cartItem);

        public async Task<List<CartItem>> GetCartItemsAsync() 
            => await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();
    }
}