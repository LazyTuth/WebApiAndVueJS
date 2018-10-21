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
        public async Task<IActionResult> GetProducts()
        {
            PagingParams pagingParams = new PagingParams {
                PageNumber = 1,
                PageSize = Int32.Parse(_config.GetSection("AppSettings:PageSize").Value)
            };

            var productFromRepo = await _productRepo.GetDataPaging(pagingParams);
            var product = _mapper.Map<IEnumerable<ProductDto>>(productFromRepo.List);
            var outputModel = new ProductOutputModel
            {
                Paging = productFromRepo.GetHeader(),
                Links = GetLinks(productFromRepo),
                Items = product.ToList()
            };
            return Ok(outputModel);
        }

        private List<LinkInfo> GetLinks(PagedList<Product> list)
        {
            var links = new List<LinkInfo>();
            if (list.HasPreviousPage)
            {
                links.Add(CreateLink("GetProducts", list.PreviousPageNumber, list.PageSize, "previousPage", "GET"));
            }
            links.Add(CreateLink("GetProducts", list.PageNumber, list.PageSize, "self", "GET"));
            if (list.HasNextPage)
            {
                links.Add(CreateLink("GetProducts", list.NextPageNumber, list.PageSize, "nextPage", "GET"));
            }
            return links;
        }

        private LinkInfo CreateLink(string routeName, int pageNumber, int pageSize, string rel, string method)
        {
            var test = _urlHelper.Link(routeName, new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                });
            return new LinkInfo
            {
                Href = _urlHelper.Link(routeName, new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }),
                Rel = rel,
                Method = method
            };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var productFromRepo = await _productRepo.Find(id);
            var product = _mapper.Map<ProductDto>(productFromRepo);
            return Ok(product);
        }
    }
}