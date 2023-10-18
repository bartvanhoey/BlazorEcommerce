

namespace Client.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient client)
        {
            _http = client;
        }

        public List<Product> Products { get; set; } = new();

        public async Task<List<Product>> GetProducts()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product");
            return response?.Data ?? new List<Product>();
        }
    }
}