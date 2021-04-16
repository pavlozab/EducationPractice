using AutoMapper;
using Dto;
using Entities;

namespace MyApi.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Address, UpdateProductDto>();
            CreateMap<UpdateProductDto, Address>();
        }
    }
}