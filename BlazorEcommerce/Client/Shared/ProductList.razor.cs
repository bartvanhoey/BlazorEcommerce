using System.Net.Http.Json;
using BlazorEcommerce.Shared;
using Client.Services.Products;
using Microsoft.AspNetCore.Components;
using Shared;

namespace BlazorEcommerce.Client.Shared;

public partial class ProductList
{
    [Inject] protected IProductService? ProductService { get; set; } 
    protected static List<Product> Products = new();

    protected override async Task OnInitializedAsync() 
       => Products = await ProductService!.GetProducts();
}