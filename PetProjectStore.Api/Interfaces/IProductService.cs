using PetProjectStore.Api.Models;
using PetProjectStore.Api.ViewModels;
using PetProjectStore.DAL.Entities;
using System.Threading.Tasks;

namespace PetProjectStore.Api.Interfaces
{
    public interface IProductService
    {
        public Task<Product> GetAsync(long id);

        public Task<long> AddAsync(Product product);

        public Task DeleteAsync(long id);

        public Task<Product> UpdateAsync(Product product);

        public Task<ProductPageViewModel> GetByPageAsync(PageModel pageModel);
    }
}
