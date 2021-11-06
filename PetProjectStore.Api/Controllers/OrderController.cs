using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetProjectStore.Api.ViewModels;
using PetProjectStore.Api.Exceptions;
using PetProjectStore.Api.Interfaces;
using PetProjectStore.DAL.Entities;
using System.Threading.Tasks;
using PetProjectStore.Api.Dtos;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;

namespace PetProjectStore.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route(Route)]
    public class OrderController : ControllerBase
    {
        private const string Route = "order";
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{pageNumber}/{pageSize}")]
        [Authorize(Policy = "all")]
        public async Task<IActionResult> GetByPageAsync(int pageNumber = 1, int pageSize = 5)
        {
            var userId = GetUserId();

            try
            {
                var orders = await _orderService.GetByPageAsync(pageNumber, pageSize, userId);

                return Ok(orders);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Wrong values");
            }
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Policy = "all")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var userId = GetUserId();

            try
            {
                var order = await _orderService.GetAsync(id, userId);

                var retval = _mapper.Map<OrderViewModel>(order);

                return Ok(retval);
            }
            catch (EntityNotFoundException)
            {
                return BadRequest("Entity not found");
            }
        }

        [HttpPost]
        [Authorize(Policy = "all")]
        public async Task<IActionResult> AddAsync([FromBody] OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            var userId = GetUserId();

            var productIds = orderDto.ProductIds;

            try
            {
                return Ok(await _orderService.AddAsync(productIds, order, userId));
            }
            catch (EntityNotFoundException)
            {
                return BadRequest("Entity not found");
            }
            catch (CartIsEmptyException)
            {
                return BadRequest("Cart is empty");
            }
        }

        [HttpDelete]
        [Authorize(Policy = nameof(UserRole.Admin))]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var userId = GetUserId();

            try
            {
                await _orderService.DeleteAsync(id, userId);

                return Ok();
            }
            catch (EntityNotFoundException)
            {
                return BadRequest("Entity not found");
            }
        }

        private string GetUserId()
        {
            return User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier)?.Value;        
        }
    }
}
