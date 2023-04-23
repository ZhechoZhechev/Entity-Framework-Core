namespace PetStoreWorkshop.Web.ViewModels.Product;

using PetStoreWorkshop.Data.Models;
using PetStoreWorkshop.Services.Mapping;

public class ListCategoriesOnCreateViewModel : IMapFrom<Category>
{
    public string Id { get; set; }

    public string Name { get; set; }
}
