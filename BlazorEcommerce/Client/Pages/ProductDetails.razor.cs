using Client.Services.Products;

namespace BlazorEcommerce.Client.Pages
{
    public class ProductDetailsBase : ComponentBase
    {
    
        [Inject] protected IProductService? ProductService { get; set; }

        protected Product? product = null;

        [Parameter] public int Id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
           product = (await ProductService!.GetProducts()).Find(p => p.Id == Id)!;
        }


        


    }
}