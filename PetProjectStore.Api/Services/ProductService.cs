using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetProjectStore.Api.Exceptions;
using PetProjectStore.Api.Interfaces;
using PetProjectStore.Api.Models;
using PetProjectStore.Api.ViewModels;
using PetProjectStore.DAL.Entities;
using PetProjectStore.DAL.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetProjectStore.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly StoreContext _storeContext;
        private readonly IMapper _mapper;

        public ProductService(StoreContext storeContext, IMapper mapper)
        {
            _storeContext = storeContext;
            _mapper = mapper;
        }

        public async Task<long> AddAsync(Product product)
        {
            if (product.Cost == 0)
                throw new CostIsNullException();

            await _storeContext.Products.AddAsync(product);
            await _storeContext.SaveChangesAsync();

            return product.Id;
        }

        public async Task DeleteAsync(long id)
        {
            var temp = await _storeContext.Products.FindAsync(id);

            if (temp == null)
            {
                throw new EntityNotFoundException();
            }

            _storeContext.Products.Remove(temp);
            await _storeContext.SaveChangesAsync();
        }

        public async Task<Product> GetAsync(long id)
        {
            var temp = await _storeContext.Products.FindAsync(id);

            if (temp == null)
            {
                throw new EntityNotFoundException();
            }

            return temp;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _storeContext.Products.Attach(product);

            if (await _storeContext.Products.FirstOrDefaultAsync(f => f.Id == product.Id) == null)
            {
                throw new EntityNotFoundException();
            }

            _storeContext.Products.Update(product);
            await _storeContext.SaveChangesAsync();

            return product;
        }

        public async Task<ProductPageViewModel> GetByPageAsync(PageModel pageModel)
        {
            if (pageModel.PageNumber == 0 || pageModel.PageSize == 0)
                throw new ArgumentNullException();

            var products = _storeContext.Products;

            var count = products.Count();

            var items = await products.Skip((pageModel.PageNumber - 1) * pageModel.PageSize).Take(pageModel.PageSize).ToArrayAsync();

            var productViewModel = _mapper.Map<IReadOnlyCollection<ProductViewModel>>(items);

            PageViewModel pageViewModel = new PageViewModel(count, pageModel.PageNumber, pageModel.PageSize);

            ProductPageViewModel productPageViewModel = new ProductPageViewModel
            {
                PageViewModel = pageViewModel,
                ProductViewModel = productViewModel
            };

            return productPageViewModel;
        }
    }
}
