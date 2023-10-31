

using Blazored.LocalStorage;

namespace Client.Services.Cart
{
    public class CartService : ICartService
    {

        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;

        public CartService(ILocalStorageService localStorageService, HttpClient httpClient)
        {
            _localStorage = localStorageService;
            _httpClient = httpClient;
        }

        public event Action? OnChange;

        public async Task AddToCartAsync(CartItem cartItem)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();
            cart.Add(cartItem);
            await _localStorage.SetItemAsync("cart", cart);
            OnChange.Invoke();
        }

        public async Task DeleteProductFromCartAsync(int id, int productTypeId)
        {
            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cartItems == null) return;

            var cartItem = cartItems.Find(x => x.ProductId == id && x.ProductTypeId == productTypeId);
            if (cartItem == null) return;
            
            if (cartItems.Remove(cartItem)){
                 await _localStorage.SetItemAsync("cart", cartItems);
                 OnChange?.Invoke();
            }
        }

        public async Task<List<CartItem>> GetCartItemsAsync()
            => await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();

        public async Task<List<CartProductResponse>> GetCartProductsAsync()
        {
            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            var response = await _httpClient.PostAsJsonAsync("api/cart/products", cartItems);
            var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();
            return cartProducts.Data;
        }
    }
}