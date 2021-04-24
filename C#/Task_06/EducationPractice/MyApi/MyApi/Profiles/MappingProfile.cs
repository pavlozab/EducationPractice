using System.Collections.Generic;
using AutoMapper;
using Dto;
using Entities;
using Services;

namespace MyApi.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Address, CreateAddressDto>();
            CreateMap<CreateAddressDto, Address>();
            CreateMap<Address, UpdateAddressDto>();
            CreateMap<UpdateAddressDto, Address>();
            CreateMap<Address, AddressResponseDto>();

            CreateMap<Order, CreateOrderDto>();
            CreateMap<CreateOrderDto, Order>();
            CreateMap<Order, OrderResponseDto>();
            CreateMap<OrderResponseDto, Order>();
        }
    }
}