namespace Client.Services.ProductTypes
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly HttpClient _http;

        public ProductTypeService(HttpClient http) 
            => _http = http;

        public List<ProductType> ProductTypes { get; set; } = new List<ProductType>();

        public event Action OnChange;

        public async Task GetProductTypesAsync()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/producttype");
            ProductTypes = response.Data;

        }
    }
}