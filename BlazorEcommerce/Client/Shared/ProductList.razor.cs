using System.Net.Http.Json;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Components;
using Shared;

namespace BlazorEcommerce.Client.Shared;

public partial class ProductList
{
    [Inject] public HttpClient Http { get; set; }
    protected static List<Product> Products = new();

    protected override async Task OnInitializedAsync()
    {
        var response = await Http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product") ?? new ServiceResponse<List<Product>>();
        if (response.Success) Products = response.Data ?? new List<Product>();

    }
}