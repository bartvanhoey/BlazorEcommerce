using Shared;

namespace Server.Services.Products
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProductsAsync();
    }
}