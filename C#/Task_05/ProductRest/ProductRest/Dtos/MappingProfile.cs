using AutoMapper;
using ProductRest.Entities;

namespace ProductRest.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, CreateProductDto>();
            CreateMap<CreateProductDto, Product>();
        }
    }
}