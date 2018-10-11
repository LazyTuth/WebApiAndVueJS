using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DemoApp.API.Data;
using DemoApp.API.Dtos;
using DemoApp.API.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.API.Controllers
{
    [Authorize]
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        public ProductController(IRepository<Product> productRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productRepo = productRepo;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productFromRepo = await _productRepo.GetAll();
            var product = _mapper.Map<IEnumerable<ProductDto>>(productFromRepo);
            return Ok(product);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var productFromRepo = await _productRepo.Find(id);
            var product = _mapper.Map<ProductDto>(productFromRepo);
            return Ok(product);
        }
    }
}