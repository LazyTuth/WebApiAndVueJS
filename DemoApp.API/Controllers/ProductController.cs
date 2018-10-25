using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DemoApp.API.Data;
using DemoApp.API.Dtos;
using DemoApp.API.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DemoApp.API.Controllers
{
    [Authorize]
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        private readonly IConfiguration _config;
        public ProductController(IRepository<Product> productRepo, IMapper mapper, IUrlHelper urlHelper, IConfiguration config)
        {
            _config = config;
            _urlHelper = urlHelper;
            _mapper = mapper;
            _productRepo = productRepo;
        }

        [HttpGet(Name="GetProducts")]
        public async Task<IActionResult> GetProducts(int? pageNumber = 1)
        {
            PagingParams pagingParams = new PagingParams {
                PageNumber = pageNumber.Value,
                PageSize = Int32.Parse(_config.GetSection("AppSettings:PageSize").Value)
            };

            var productFromRepo = await _productRepo.GetDataPaging(pagingParams);
            var product = _mapper.Map<IEnumerable<ProductDto>>(productFromRepo.List);
            var outputModel = new ProductOutputModel
            {
                Paging = productFromRepo.GetHeader(),
                Items = product.ToList()
            };
            return Ok(outputModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var productFromRepo = await _productRepo.Find(id);
            var product = _mapper.Map<ProductDto>(productFromRepo);
            return Ok(product);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateProductDto dataToCreate)
        {
            var productFromRepo = await _productRepo.GetSingleWithCondition(s => s.ProductName == dataToCreate.ProductName);
            if (productFromRepo != null)
                return BadRequest("Product Exists !!!");

            var product = _mapper.Map<Product>(dataToCreate);
            var isInsert = await _productRepo.InsertAsync(product);
            if (isInsert != null)
            {
                return CreatedAtRoute("GetProducts", null);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPatch("edit")]
        public async Task<IActionResult> Edit(EditProductDto dataToEdit)
        {
            var product = _mapper.Map<Product>(dataToEdit);
            var isUpdate = await _productRepo.UpdateAsync(product, dataToEdit.ProductId);
            if (isUpdate != null)
            {
                return CreatedAtRoute("GetProducts", null);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int productId)
        {
            var product = await _productRepo.Find(productId);
            int isDelete = await _productRepo.DeleteAsync(product);
            if (isDelete > 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}