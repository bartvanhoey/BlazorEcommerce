using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services.Categories
{
    public interface ICategoryService
    {

        
          Task<ServiceResponse<List<Category>>> GetCategoriesAsync();
    }
}