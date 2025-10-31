using AutoMapper;
using OrderService.Dto;
using OrderService.Models;

namespace OrderService.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Order,OrderDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<OrderItem,OrderItemDto>();
            CreateMap<OrderItemDto, OrderItem>();
        }
    }
}