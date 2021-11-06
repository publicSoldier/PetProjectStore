using AutoMapper;
using PetProjectStore.Api.Dtos;
using PetProjectStore.Api.ViewModels;
using PetProjectStore.DAL.Entities;

namespace PetProjectStore.Api.Mapper.Profiles
{
    public class EntityMapperProfile : Profile
    {
        public EntityMapperProfile()
        {
            RegisterOrderMaps();
            RegisterProductMaps();
            RegisterUserMaps();
        }

        public void RegisterProductMaps()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();
            CreateMap<Product, ProductViewModel>();
        }

        public void RegisterOrderMaps()
        {
            CreateMap<OrderDto, Order>();
            CreateMap<Order, OrderViewModel>();
        }

        public void RegisterUserMaps()
        {
            CreateMap<RegistrationDto, User>();
            CreateMap<LogInDto, User>();
        }
    }
}
