using AutoMapper;
using ProductRest.Dto;
using ProductRest.Entities;

namespace ProductRest.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, UpdateProductDto>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}