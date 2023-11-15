

using Blazored.LocalStorage;

namespace Client.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public CartService(ILocalStorageService localStorageService, HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _localStorage = localStorageService;
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public event Action? OnChange;




        public async Task AddToCartAsync(CartItem cartItem)
        {
            if (await IsUserAuthenticated())
            {
                Console.WriteLine("User already authenticated");
            }
            else
            {
                Console.WriteLine("User NOT authenticated");
            }

            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();
            var sameItem = cart.FirstOrDefault(x => x.ProductId == cartItem.ProductId && x.ProductTypeId == cartItem.ProductTypeId);
            if (sameItem == null)
                cart.Add(cartItem);
            else
                sameItem.Quantity += cartItem.Quantity;
            await _localStorage.SetItemAsync("cart", cart);
            OnChange?.Invoke();
        }

        private async Task<bool> IsUserAuthenticated() => (await _authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity?.IsAuthenticated ?? false;

        public async Task DeleteProductFromCartAsync(int id, int productTypeId)
        {
            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cartItems == null) return;

            var cartItem = cartItems.Find(x => x.ProductId == id && x.ProductTypeId == productTypeId);
            if (cartItem == null) return;

            if (cartItems.Remove(cartItem))
            {
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
            return cartProducts?.Data ?? new List<CartProductResponse>();
        }

        public async Task StoreCartItemsAsync(bool emptyLocalCart = false)
        {
            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cartItems == null) return;

            await _httpClient.PostAsJsonAsync("api/cart", cartItems);

            if (emptyLocalCart)
            {
                await _localStorage.RemoveItemAsync("cart");
            }




        }

        public async Task UpdateQuantityAsync(CartProductResponse product)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null) return;

            var cartItem = cart.Find(p => p.ProductId == product.ProductId && p.ProductTypeId == product.ProductTypeId);
            if (cartItem == null) return;

            cartItem.Quantity = product.Quantity;
            await _localStorage.SetItemAsync("cart", cart);
        }
    }
}