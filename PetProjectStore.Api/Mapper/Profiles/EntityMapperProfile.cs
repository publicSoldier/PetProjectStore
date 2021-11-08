using AutoMapper;
using PetProjectStore.Api.Models;
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
            CreateMap<ProductModel, Product>();
            CreateMap<Product, ProductModel>();
            CreateMap<Product, ProductViewModel>();
        }

        public void RegisterOrderMaps()
        {
            CreateMap<OrderModel, Order>();
            CreateMap<Order, OrderViewModel>();

            CreateMap<DeliveryTypeModel, DeliveryType>();
        }

        public void RegisterUserMaps()
        {
            CreateMap<RegistrationModel, User>();
            CreateMap<LogInModel, User>();
        }
    }
}
