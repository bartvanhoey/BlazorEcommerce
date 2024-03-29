using Client.Services.Orders;

namespace BlazorEcommerce.Client.Pages
{
    public class CartBase : ComponentBase
    {
        protected List<CartProductResponse> CartProducts = new();
        protected string Message = "Loading cart...";
        protected bool IsAuthenticated = false;

        [Inject] protected ICartService? CartService { get; set; }

        [Inject] protected IAuthService? AuthService { get; set; }

        [Inject] protected IOrderService? OrderService { get; set; }

        [Inject] protected NavigationManager? NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsAuthenticated = await AuthService.IsUserAuthenticatedAsync();
            await LoadCartAsync();
        }

        protected async Task RemoveProductFromCartAsync(int productId, int productTypeId)
        {
            await CartService!.RemoveProductFromCartAsync(productId, productTypeId);
            await LoadCartAsync();
        }

        private async Task LoadCartAsync()
        {
            await CartService!.GetCartItemsCountAsync();
            CartProducts = await CartService!.GetCartProductsAsync();

            if (CartProducts == null || CartProducts.Count == 0)
            {
                Message = "Your cart is empty";
                CartProducts = new List<CartProductResponse>();
            }
            else
            {
                CartProducts = await CartService.GetCartProductsAsync();
            }
        }

        protected async Task UpdateQuantityAsync(ChangeEventArgs changeEventArgs, CartProductResponse product)
        {
            product.Quantity = int.Parse(changeEventArgs?.Value?.ToString() ?? "1");
            if (product.Quantity < 1) product.Quantity = 1;
            await CartService!.UpdateQuantityAsync(product);
        }

        protected async Task PlaceOrderAsync()
        {
            string url = await OrderService!.PlaceOrderAsync();
            // await CartService!.GetCartItemsCountAsync();
            // OrderPlaced =true;
            NavigationManager!.NavigateTo(url, false);
        }


    }
}