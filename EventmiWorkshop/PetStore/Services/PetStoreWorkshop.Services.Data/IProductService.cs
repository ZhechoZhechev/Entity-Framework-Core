namespace PetStoreWorkshop.Services.Data;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PetStoreWorkshop.Data.Models;

public interface IProductService
{
    IQueryable<Product> GetAllProductsByName(string productName = "");

    IQueryable<Product> GetAllProductsByCategory(string categoryName = "");

    ICollection<string> GetAllProductCategories();

    Task<Product> GetProductByIdAsync(string productId);

    Task AdProductAsync(Product product);
}
