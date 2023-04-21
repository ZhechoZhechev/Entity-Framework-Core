namespace PetStoreWorkshop.Web.ViewModels.Product;

using PetStoreWorkshop.Data.Models;
using PetStoreWorkshop.Services.Mapping;

public class ListAllProductViewModel : IMapFrom<Product>
{
    public string Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public string CategoryName { get; set; }
}
