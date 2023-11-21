namespace Client.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly NavigationManager _navigationManager;

        public OrderService(HttpClient http, AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager)
        {
            _http = http;
            _authenticationStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task PlaceOrderAsync()
        {
            if (await IsUserAuthenticatedAsync())
            {
                await _http.PostAsync("api/order", null);
            }
            else
            {
                _navigationManager!.NavigateTo("login", false);
            }
        }

        private async Task<bool> IsUserAuthenticatedAsync()
            => (await _authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity?.IsAuthenticated ?? false;

    }
}