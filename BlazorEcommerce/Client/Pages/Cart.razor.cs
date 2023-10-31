namespace BlazorEcommerce.Client.Pages
{
    public class CartBase : ComponentBase
    {
       protected List<CartProductResponse> CartProducts = new();
       protected string Message = "Loading cart...";
       
        [Inject] protected ICartService? CartService { get; set; }


        protected override async Task OnInitializedAsync()
        {
            if ((await CartService!.GetCartItemsAsync()).Count==0)
            {
                Message= "Your cart is empty";
                CartProducts = new List<CartProductResponse>();                
            }
            else
            {
                CartProducts = await CartService.GetCartProductsAsync();
            }
        }



    }
}