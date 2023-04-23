namespace PetStoreWorkshop.Web.ViewModels.Product;

using System.ComponentModel.DataAnnotations;

using PetStoreWorkshop.Data.Models;
using PetStoreWorkshop.Data.Models.Common;
using PetStoreWorkshop.Services.Mapping;

public class ProductInputModel : IMapTo<Product>
{
    [Required(AllowEmptyStrings = false, ErrorMessage = ModelConstants.EmptyNameErrorMassage)]
    [StringLength(ModelConstants.ProductNameLength, MinimumLength = ModelConstants.ProductMinLength)]
    public string Name { get; set; }

    [Required]
    [Range(typeof(decimal), ModelConstants.ProductPriceMinValue, ModelConstants.ProductPriceMaxValue)]
    public decimal Price { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = ModelConstants.EmptyImageURLErrorMassage)]
    public string ImageURL { get; set; }

    [Required]
    public int CategoryId { get; set; }
}
