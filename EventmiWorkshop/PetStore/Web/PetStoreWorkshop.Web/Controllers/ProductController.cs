namespace PetStoreWorkshop.Web.Controllers;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetStoreWorkshop.Common;
using PetStoreWorkshop.Data.Models;
using PetStoreWorkshop.Services.Data;
using PetStoreWorkshop.Services.Mapping;
using PetStoreWorkshop.Web.ViewModels.Product;

public class ProductController : BaseController
{
    private readonly IProductService productService;
    private readonly ICategoryService categoryService;

    public ProductController(
        IProductService productService,
        ICategoryService categoryService)
    {
        this.productService = productService;
        this.categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        Product product = await this.productService.GetProductByIdAsync(id);

        if (product == null)
        {
            this.RedirectToAction("Error", "Home");
        }

        DetailsProductViewModel detailViewModel =
            AutoMapperConfig.MapperInstance.Map<DetailsProductViewModel>(product);

        return this.View(detailViewModel);
    }

    [HttpPost]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public async Task<IActionResult> Create(ProductInputModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        if (!this.categoryService.IfCategoryExists(model.CategoryId))
        {
            return this.View(model);
        }

        Product product = AutoMapperConfig.MapperInstance.Map<Product>(model);
        await this.productService.AdProductAsync(product);

        return this.RedirectToAction("All", "Product");
    }

    [HttpGet]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public IActionResult Create()
    {
        ICollection<ListCategoriesOnCreateViewModel> allCategories = this.categoryService.All()
            .To<ListCategoriesOnCreateViewModel>().ToArray();

        return this.View(allCategories);
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
