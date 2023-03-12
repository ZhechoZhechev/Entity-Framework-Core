namespace ProductShop;

using AutoMapper;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;


public class ProductShopProfile : Profile
{
    public ProductShopProfile() 
    {
        this.CreateMap<ImportUserDTO, User>();
        this.CreateMap<ImportProductDTO, Product>();
        this.CreateMap<ImportCategorieDTO, Category>();
        this.CreateMap<ImportCategoryProductDTO, CategoryProduct>();

        this.CreateMap<Product, ExportProductsInRangeDTO>()
            .ForMember(d => d.SellerFullName, mo => mo.MapFrom(s => string.Concat(s.Seller.FirstName, " ", s.Seller.LastName)));

        this.CreateMap<Product, UsersSoldProductsDTO>()
            .ForMember(d => d.BuyerFirstName, mo => mo.MapFrom(s => s.Buyer.FirstName))
            .ForMember(d => d.BuyerLastName, mo => mo.MapFrom(s => s.Buyer.LastName));

        this.CreateMap<User, UsersWithSoldItemsDTO>()
            .ForMember(d => d.ProductInfo, mo => mo.MapFrom(s => s.ProductsSold.Where(x => x.BuyerId.HasValue)));
    }
}
