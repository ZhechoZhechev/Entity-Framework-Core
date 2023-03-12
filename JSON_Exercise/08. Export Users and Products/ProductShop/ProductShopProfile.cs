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

        this.CreateMap<Category, CategoriesByProductsCountDTO>()
            .ForMember(d => d.ProductsCount, mo => mo.MapFrom(s => s.CategoriesProducts.Count))
            .ForMember(d => d.AveragePrice, mo => mo.MapFrom(s => Math.Round((double)s.CategoriesProducts.Average(p => p.Product.Price), 2)))
            .ForMember(d => d.TotalRevenue, mo => mo.MapFrom(s => Math.Round((double)s.CategoriesProducts.Sum(p => p.Product.Price), 2)));
    }
}
