using System.Collections.Generic;
using System.Linq;
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
    [Route("api/category")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IRepository<ProductCategory> _cateRepo;
        private readonly IMapper _mapper;

        public ProductCategoryController(IRepository<ProductCategory> cateRepo, IMapper mapper)
        {
            _cateRepo = cateRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductCate()
        {
            var productFromRepo = await _cateRepo.GetAll();
            var prdCate = _mapper.Map<IEnumerable<ProductCateDto>>(productFromRepo);
            return Ok(prdCate.OrderBy(s => s.Description));
        }
    }
}