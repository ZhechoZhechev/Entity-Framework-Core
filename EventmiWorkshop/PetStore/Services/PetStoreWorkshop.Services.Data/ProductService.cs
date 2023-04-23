namespace PetStoreWorkshop.Services.Data;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using PetStoreWorkshop.Data.Common.Repositories;
using PetStoreWorkshop.Data.Models;

public class ProductService : IProductService
{
    private const string EmptyString = "";
    private readonly IDeletableEntityRepository<Product> productRepo;

    public ProductService(IDeletableEntityRepository<Product> productRepo)
    {
        this.productRepo = productRepo;
    }

    public async Task AdProductAsync(Product product)
    {
        await this.productRepo.AddAsync(product);
        await this.productRepo.SaveChangesAsync();
    }

    public ICollection<string> GetAllProductCategories()
    {
        return this.productRepo.AllAsNoTracking()
            .Select(p => p.Category.Name).ToArray();
    }

    public IQueryable<Product> GetAllProductsByCategory(string categoryName = EmptyString)
    {
        if (categoryName != EmptyString)
        {
            return this.productRepo.AllAsNoTracking()
                .Where(p => p.Category.Name.ToLower().Contains(categoryName.ToLower()));
        }

        return this.productRepo.All();
    }

    public IQueryable<Product> GetAllProductsByName(string productName)
    {
        if (productName != null)
        {
            return this.productRepo.AllAsNoTracking()
                .Where(p => p.Name.ToLower().Contains(productName.ToLower()));
        }

        return this.productRepo.All();
    }

    public async Task<Product> GetProductByIdAsync(string productId)
    {
        return await this.productRepo.All()
            .FirstOrDefaultAsync(p => p.Id == productId);
    }
}
