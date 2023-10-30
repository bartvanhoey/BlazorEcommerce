

using Blazored.LocalStorage;

namespace Client.Services.Cart
{
    public class CartService : ICartService
    {

        private readonly ILocalStorageService _localStorage;
        public CartService(ILocalStorageService localStorageService) => _localStorage = localStorageService;

        public event Action OnChange;

        public async Task AddToCartAsync(CartItem cartItem)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();
            cart.Add(cartItem);
            await _localStorage.SetItemAsync("cart", cart);
            OnChange.Invoke();
        }

        public async Task<List<CartItem>> GetCartItemsAsync()
            => await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();
    }
}