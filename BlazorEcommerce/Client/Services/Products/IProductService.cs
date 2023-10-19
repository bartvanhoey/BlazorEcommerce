namespace Client.Services.Products
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<Product> Products { get; set; }
        Task<List<Product>> GetProductsAsync(string? categoryUrl = null);
        Task<ServiceResponse<Product>> GetProductAsync(int productId);
        Task<List<Product>> SearchProductsAsync(string searchText);
        Task<List<string>> GetProductSearchSuggestionsAsync(string searchText);
    }
}