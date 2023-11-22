using Client.Services.Orders;

namespace BlazorEcommerce.Client.Pages
{
    public class OrderDetailsBase : ComponentBase
    {
        [Inject] protected IOrderService? OrderService { get; set; }

        [Parameter] public int OrderId { get; set; }

        protected OrderDetailsResponse? Order = null;

        protected override async Task OnInitializedAsync()
            => Order = await OrderService!.GetOrderDetailsAsync(OrderId);


    }
}