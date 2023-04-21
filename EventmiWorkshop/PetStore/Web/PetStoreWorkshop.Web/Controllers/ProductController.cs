namespace PetStoreWorkshop.Web.Controllers;

using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using PetStoreWorkshop.Data.Models;
using PetStoreWorkshop.Services.Data;
using PetStoreWorkshop.Services.Mapping;
using PetStoreWorkshop.Web.ViewModels.Product;

public class ProductController : BaseController
{
    private readonly IProductService productService;

    public ProductController(IProductService productService)
    {
        this.productService = productService;
    }

    [HttpGet]
    public IActionResult All(string search)
    {
        IQueryable<Product> allProducts = this.productService.GetAllProductsByName(search);
        ICollection<string> categories = this.productService.GetAllProductCategories();

        AllProductsViewModel model = new AllProductsViewModel()
        {
            AllProducts = allProducts.To<ListAllProductViewModel>().ToArray(),
            AllCategories = categories,
            SearchQuery = search,
        };

        return this.View(model);
    }
}
