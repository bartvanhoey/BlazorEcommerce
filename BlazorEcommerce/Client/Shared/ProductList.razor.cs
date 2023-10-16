using System.Net.Http.Json;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Client.Shared;

public partial class ProductList
{
    [Inject] public HttpClient Http { get; set; }
    protected static List<Product> Products = new();

    protected override async Task OnInitializedAsync()
    {
         Products = await Http.GetFromJsonAsync<List<Product>>("api/product") ?? new List<Product>();
    }
}