namespace Client.Services.Products
{
    public interface IProductService
    {
        List<Product> Products { get; set; }
        Task<List<Product>> GetProductsAsync(string? categoryUrl = null);
        Task<ServiceResponse<Product>> GetProductAsync(int productId);
    }
}