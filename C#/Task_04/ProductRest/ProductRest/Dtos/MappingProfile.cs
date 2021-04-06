using AutoMapper;

namespace ProductRest.Dtos
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