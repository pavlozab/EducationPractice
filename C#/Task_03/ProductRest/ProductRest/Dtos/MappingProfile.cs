using AutoMapper;
using ProductRest.Dtos;

namespace ProductRest
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDto, CreateProductDto>();
            CreateMap<CreateProductDto, ProductDto>();
        }
    }
}