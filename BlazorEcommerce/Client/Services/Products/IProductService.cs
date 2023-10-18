namespace Client.Services.Products
{
    public interface IProductService
    {
        List<Product> Products { get; set; }
        Task<List<Product>> GetProductsAsync();
        Task<ServiceResponse<Product>> GetProductAsync(int productId);
    }
}