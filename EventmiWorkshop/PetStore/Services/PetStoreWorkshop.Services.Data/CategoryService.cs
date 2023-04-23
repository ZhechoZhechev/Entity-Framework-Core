namespace PetStoreWorkshop.Services.Data;

using System.Linq;
using System.Security.Cryptography.X509Certificates;
using PetStoreWorkshop.Data.Common.Repositories;
using PetStoreWorkshop.Data.Models;

public class CategoryService : ICategoryService
{
    private readonly IDeletableEntityRepository<Category> categoryRepo;

    public CategoryService(IDeletableEntityRepository<Category> categoryRepo)
    {
        this.categoryRepo = categoryRepo;
    }

    public IQueryable<Category> All()
    {
        return this.categoryRepo.AllAsNoTracking();
    }

    public bool IfCategoryExists(int id)
    {
        return this.categoryRepo.AllAsNoTracking()
            .Select(x => x.Id)
            .Contains(id);
    }
}
