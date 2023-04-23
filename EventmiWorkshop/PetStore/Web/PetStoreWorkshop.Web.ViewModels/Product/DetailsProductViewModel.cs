namespace PetStoreWorkshop.Web.ViewModels.Product;

using AutoMapper;
using PetStoreWorkshop.Data.Models;
using PetStoreWorkshop.Services.Mapping;

public class DetailsProductViewModel : IMapFrom<Product>, IHaveCustomMappings
{
    public string Name { get; set; }

    public decimal Price { get; set; }

    public string ImageURL { get; set; }

    public string CategoryName { get; set; }

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<Product, DetailsProductViewModel>()
            .ForMember(x => x.CategoryName, mo => mo.MapFrom(x => x.Category.Name));
    }
}
