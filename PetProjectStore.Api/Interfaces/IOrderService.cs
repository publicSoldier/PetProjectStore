using PetProjectStore.Api.Models;
using PetProjectStore.Api.ViewModels;
using PetProjectStore.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetProjectStore.Api.Interfaces
{
    public interface IOrderService
    {

        public Task<Order> GetAsync(long id, string userId);

        public Task<long> AddAsync(IReadOnlyCollection<long> productIds, Order order, string userId);

        public Task DeleteAsync(long id, string userId);

        public Task<OrderPageViewModel> GetByPageAsync(PageModel pageModel, string userId);
    }
}
