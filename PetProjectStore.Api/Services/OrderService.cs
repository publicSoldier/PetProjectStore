using PetProjectStore.Api.Interfaces;
using PetProjectStore.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetProjectStore.DAL.EntityFramework;
using PetProjectStore.Api.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using PetProjectStore.Api.ViewModels;
using AutoMapper;
using PetProjectStore.Api.Models;

namespace PetProjectStore.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly StoreContext _storeContext;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public OrderService(StoreContext storeContext, IProductService productService, IMapper mapper)
        {
            _storeContext = storeContext;
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<long> AddAsync(IReadOnlyCollection<long> productIds, Order order, string userId)
        {
            order.Products = await GetProductsAsync(productIds);
            order.CustomerId = userId;

            if (order.Products.Count == 0)
                throw new CartIsEmptyException();

            foreach (var product in order.Products)
            {
                order.TotalCost += product.Cost;
            }

            order.Date = System.DateTime.Now;

            await _storeContext.Orders.AddAsync(order);
            await _storeContext.SaveChangesAsync();

            return order.Id;
        }

        public async Task DeleteAsync(long id, string userId)
        {
            var temp = await _storeContext.Orders.FirstOrDefaultAsync(f => f.Id == id);

            if (temp == null)
            {
                throw new EntityNotFoundException();
            }

            _storeContext.Orders.Remove(temp);
            await _storeContext.SaveChangesAsync();
        }

        public async Task<Order> GetAsync(long id, string userId)
        {

            var entity = await _storeContext.Orders
                .Include(i => i.Products)
                .FirstOrDefaultAsync(w => w.Id == id && w.CustomerId == userId);

            if (entity == null)          
                throw new EntityNotFoundException();           

            return entity;
        }

        public async Task<OrderPageViewModel> GetByPageAsync(PageModel pageModel, string userId)
        {
            if (pageModel.PageNumber == 0 || pageModel.PageSize == 0)
                throw new ArgumentNullException();

            var orders = _storeContext.Orders
                .Include(i => i.Products)
                .Where(w => w.CustomerId == userId);

            var count = orders.Count();

            var items = await orders.Skip((pageModel.PageNumber - 1) * pageModel.PageSize).Take(pageModel.PageSize).ToArrayAsync();

            var orderViewModel = _mapper.Map<IReadOnlyCollection<OrderViewModel>>(items);

            PageViewModel pageViewModel = new PageViewModel(count, pageModel.PageNumber, pageModel.PageSize);

            OrderPageViewModel orderPageViewModel = new OrderPageViewModel
            {
                OrderViewModel = orderViewModel,
                PageViewModel = pageViewModel
            };

            return orderPageViewModel;
        }

        private async Task<IReadOnlyCollection<Product>> GetProductsAsync(IReadOnlyCollection<long> productIds)
        {
            var products = new List<Product>();

            foreach (var id in productIds)
            {
                products.Add(await _productService.GetAsync(id));
            }

            return products;
        }
    }
}
