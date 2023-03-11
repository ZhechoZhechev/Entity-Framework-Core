﻿namespace ProductShop;

using AutoMapper;
using ProductShop.DTOs.Import;
using ProductShop.Models;


public class ProductShopProfile : Profile
{
    public ProductShopProfile() 
    {
        this.CreateMap<ImportUserDTO, User>();
        this.CreateMap<ImportProductDTO, Product>();
    }
}