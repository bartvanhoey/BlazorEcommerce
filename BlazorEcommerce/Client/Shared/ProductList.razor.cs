using Client.Services.Products;
using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Client.Shared;

public partial class ProductList
{
    // [Inject] protected IProductService? ProductService { get; set; } 

    [Parameter]
    public List<Product>? Products { get; set; }

    // protected override async Task OnInitializedAsync()
    // {
    //     // var products = await ProductService!.GetProductsAsync();
    //     Products = products;
    // }
}