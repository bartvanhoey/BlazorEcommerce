using Server.Data;
using Shared;

namespace Server.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly DataContext _db;

        public ProductService(DataContext dataContext)
        {
            _db = dataContext;
        }



        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var serviceResponse = new ServiceResponse<List<Product>>() { Data = await _db.Products.ToListAsync() };
            return serviceResponse;
        }


    }
}