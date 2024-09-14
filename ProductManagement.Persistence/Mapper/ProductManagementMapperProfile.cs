using AutoMapper;
using ProductManagement.Core.DTOs.Product;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Persistence.Mapper;

public class ProductManagementMapperProfile : Profile
{
    public ProductManagementMapperProfile()
    {
        //Product
        CreateMap<Product, ListProductDto>();
        CreateMap<AddProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
    }
}