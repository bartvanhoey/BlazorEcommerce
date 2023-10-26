using System.Reflection.Metadata.Ecma335;
using Client.Services.Products;
using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Client.Shared;

public partial class ProductList
{
    [Parameter]
    public List<Product>? Products { get; set; }

    [Inject] protected IProductService? ProductService { get; set; }


    protected string GetPriceText(Product product)
    {
        var variants = product.Variants;
        if (variants.Count == 0)
        {
            return string.Empty;
        }
        else if (variants.Count == 1)
        {
            return $"${variants[0].Price}";
        }
        decimal minPrice = variants.Min(v => v.Price);
        return $"Starting ad ${minPrice}";
    }

}