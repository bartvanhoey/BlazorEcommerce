
namespace Client.Services.Products
{
    public class ProductService : IProductService
    {
        public List<Product> Products { get; set; } = new();

        public Task<List<Product>> GetProducts()
        {
            throw new NotImplementedException();
        }
    }
}