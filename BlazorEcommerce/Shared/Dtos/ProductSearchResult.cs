using BlazorEcommerce.Shared;

namespace Shared.Dtos
{
    public class ProductSearchResult
    {
            public ProductSearchResult(IEnumerable<Product> products, int currentPage, double pageCount)
            {
                Products = products.ToList();
                CurrentPage = currentPage;
                Pages =(int) pageCount;
            }

        	public List<Product> Products { get; set; } = new List<Product>();
            public int Pages { get; set; }
            public int CurrentPage { get; set; }
    }
}