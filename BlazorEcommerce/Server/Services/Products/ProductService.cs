using Azure;
using Server.Data;
using Shared;

namespace Server.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly DataContext _db;

        public ProductService(DataContext dataContext) => _db = dataContext;

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId);
            var response = new ServiceResponse<Product>();
            if (product == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this product does not exist.";
            }
            else
            {
                response.Data = product;
            }

            return response;



        }

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
            => new ServiceResponse<List<Product>>() { Data = await _db.Products.ToListAsync() };


    }
}