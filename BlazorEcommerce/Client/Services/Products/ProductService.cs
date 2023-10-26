


using Shared.Dtos;

namespace Client.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient client) => _http = client;

        public List<Product> Products { get; set; } = new();
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;

        public event Action ProductsChanged;

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
            return response;
        }

        public async Task<List<Product>> GetProductsAsync(string? categoryUrl = null)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>(categoryUrl == null ? "api/product/featured" : $"api/product/category/{categoryUrl}");
            CurrentPage = 1;
            PageCount = 0;

            return response?.Data ?? new List<Product>();

        }

        public async Task<List<string>> GetProductSearchSuggestionsAsync(string searchText)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchsuggestions/{searchText}");
            return response?.Data ?? new List<string>();
        }

        public async Task<ProductSearchResult> SearchProductsAsync(string searchText, int page)
        {
            LastSearchText = searchText;

            var response = await _http.GetFromJsonAsync<ServiceResponse<ProductSearchResult>>($"api/product/search/{searchText}/{page}");
            if (response != null)
            {
                Products = response?.Data?.Products ?? new List<Product>();
                CurrentPage = response?.Data?.CurrentPage == null ? 0 : response.Data.CurrentPage;
                PageCount = response?.Data?.Pages == null ? 0 : response.Data.Pages;
            }

            return response?.Data ?? new ProductSearchResult();
        }
    }
}