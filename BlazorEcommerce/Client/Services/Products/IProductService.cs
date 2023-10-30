using Shared.Dtos;

namespace Client.Services.Products
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<Product> Products { get; set; }
        Task<List<Product>> GetProductsAsync(string? categoryUrl = null);
        Task<ServiceResponse<Product>> GetProductAsync(int productId);
        Task<ProductSearchResult> SearchProductsAsync(string searchText, int page);
        Task<List<string>> GetProductSearchSuggestionsAsync(string searchText);
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        string LastSearchText { get; set; }
    }
}