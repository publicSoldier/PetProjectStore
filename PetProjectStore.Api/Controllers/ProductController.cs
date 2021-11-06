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

namespace PetProjectStore.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route(Route)]
    public class ProductController : ControllerBase
    {
        private const string Route = "product";
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{pageNumber}/{pageSize}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByPageAsync(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                var products = await _productService.GetByPageAsync(pageNumber, pageSize);

                return Ok(products);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Wrong values");
            }
        }

        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync(long id)
        {
            try
            {
                var product = await _productService.GetAsync(id);

                var retval = _mapper.Map<ProductViewModel>(product);

                return Ok(retval);
            }
            catch (EntityNotFoundException)
            {
                return BadRequest("Entity not found");
            }
        }

        [HttpPost]
        [Authorize(Policy = nameof(UserRole.Admin))]
        public async Task<IActionResult> AddAsync([FromBody] ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            try
            {
                await _productService.AddAsync(product);

                return Ok(product.Id);
            }
            catch (CostIsNullException)
            {
                return BadRequest("Cost is zero");
            }
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Policy = nameof(UserRole.Admin))]
        public async Task<IActionResult> UpdateAsync(long id, [FromBody] ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            product.Id = id;

            try
            {
                await _productService.UpdateAsync(product);

                return Ok(product.Id);
            }
            catch (EntityNotFoundException)
            {
                return BadRequest("Entity not found");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = nameof(UserRole.Admin))]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            try
            {
                await _productService.DeleteAsync(id);

                return Ok();
            }
            catch (EntityNotFoundException)
            {
                return BadRequest("Entity not found");
            }
        }
    }
}
