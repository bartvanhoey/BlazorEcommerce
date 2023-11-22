using Client.Services.Orders;

namespace BlazorEcommerce.Client.Pages
{
    public class OrdersBase : ComponentBase
    {
        [Inject] protected IOrderService? OrderService { get; set; }

        public List<OrderOverviewResponse>? Orders { get; set; } = null;

        protected override async Task OnInitializedAsync() 
            => Orders = await OrderService!.GetOrdersAsync();
    }
}