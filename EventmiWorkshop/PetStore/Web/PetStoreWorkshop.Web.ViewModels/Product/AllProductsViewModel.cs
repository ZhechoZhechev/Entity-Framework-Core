namespace PetStoreWorkshop.Web.ViewModels.Product;

using System.Collections.Generic;

public class AllProductsViewModel
{
    public ICollection<ListAllProductViewModel> AllProducts { get; set; }

    public ICollection<string> AllCategories { get; set; }

    public string SearchQuery { get; set; }
}
