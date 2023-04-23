namespace PetStoreWorkshop.Services.Data
{
    using System.Linq;

    using PetStoreWorkshop.Data.Models;

    public interface ICategoryService
    {
        IQueryable<Category> All();

        bool IfCategoryExists(int id);
    }
}
