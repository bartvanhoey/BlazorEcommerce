using Client.Services.Products;

namespace BlazorEcommerce.Client.Pages
{
    public class ProductDetailsBase : ComponentBase
    {

        [Inject] protected IProductService? ProductService { get; set; }

        protected Product? product = null;
        protected string message = string.Empty;

        [Parameter] public int Id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            message = "Loading products";
            var result = await ProductService!.GetProductAsync(Id)!;
            if (result.Success)
            {
                product = result.Data;
            }
            else
                message = result.Message;
        }





    }
}