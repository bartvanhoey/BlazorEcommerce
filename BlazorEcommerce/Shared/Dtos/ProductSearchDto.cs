using BlazorEcommerce.Shared;

namespace Shared.Dtos
{
    public class ProductSearchDto
    {
        	public List<Product> Products { get; set; } = new List<Product>();
            public int Pages { get; set; }
            public int CurrentPage { get; set; }
    }
}