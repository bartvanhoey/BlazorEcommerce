

using Blazored.LocalStorage;

namespace Client.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        
        private readonly IAuthService _authService;

        public CartService(ILocalStorageService localStorageService, HttpClient httpClient, IAuthService authService )
        {
            _authService = authService;
            _localStorage = localStorageService;
            _http = httpClient;
        
        }

        public event Action? OnChange;

        public async Task AddToCartAsync(CartItem cartItem)
        {
            if (await _authService.IsUserAuthenticatedAsync())
            {
                var response = await _http.PostAsJsonAsync("api/cart/add", cartItem);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();
                var sameItem = cart.FirstOrDefault(x => x.ProductId == cartItem.ProductId && x.ProductTypeId == cartItem.ProductTypeId);
                if (sameItem == null)
                    cart.Add(cartItem);
                else
                    sameItem.Quantity += cartItem.Quantity;
                await _localStorage.SetItemAsync("cart", cart);
            }

            await GetCartItemsCountAsync();
        }

        

        public async Task RemoveProductFromCartAsync(int id, int productTypeId)
        {
            if (await _authService.IsUserAuthenticatedAsync())
            {
                await _http.DeleteAsync($"api/cart/{id}/{productTypeId}");
            }
            else
            {
                var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cartItems == null) return;

                var cartItem = cartItems.Find(x => x.ProductId == id && x.ProductTypeId == productTypeId);
                if (cartItem == null) return;

                if (cartItems.Remove(cartItem))
                {
                    await _localStorage.SetItemAsync("cart", cartItems);
                }
            }
            await GetCartItemsCountAsync();
        }

        public async Task<List<CartProductResponse>> GetCartProductsAsync()
        {
            if (await _authService.IsUserAuthenticatedAsync())
            {
                return (await _http.GetFromJsonAsync<ServiceResponse<List<CartProductResponse>>>("api/cart"))?.Data ?? new List<CartProductResponse>();
            }

            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cartItems == null) return new List<CartProductResponse>();

            var response = await _http.PostAsJsonAsync("api/cart/products", cartItems);
            var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();
            return cartProducts?.Data ?? new List<CartProductResponse>();
        }

        public async Task StoreCartItemsAsync(bool emptyLocalCart = false)
        {
            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cartItems == null) return;

            await _http.PostAsJsonAsync("api/cart", cartItems);

            if (emptyLocalCart)
            {
                await _localStorage.RemoveItemAsync("cart");
            }
        }

        public async Task UpdateQuantityAsync(CartProductResponse product)
        {
            if (await _authService.IsUserAuthenticatedAsync())
            {
                var request = new CartItem { ProductId = product.ProductId, Quantity = product.Quantity, ProductTypeId = product.ProductTypeId };
                var response = await _http.PutAsJsonAsync("api/cart/update-quantity", request);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null) return;

                var cartItem = cart.Find(p => p.ProductId == product.ProductId && p.ProductTypeId == product.ProductTypeId);
                if (cartItem == null) return;

                cartItem.Quantity = product.Quantity;
                await _localStorage.SetItemAsync("cart", cart);
            }
        }

        public async Task GetCartItemsCountAsync()
        {
            if (await _authService.IsUserAuthenticatedAsync())
            {
                var response = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                var count = response?.Data ?? 0;
                await _localStorage.SetItemAsync("cartItemsCount", count);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                await _localStorage.SetItemAsync("cartItemsCount", cart != null ? cart.Count : 0);
            }
            OnChange?.Invoke();
        }
    }
}