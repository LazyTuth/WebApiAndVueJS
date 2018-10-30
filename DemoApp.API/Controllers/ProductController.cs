using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DemoApp.API.Data;
using DemoApp.API.Dtos;
using DemoApp.API.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DemoApp.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        #region Register Services
        private const string _productImagesFolder = "Images/Products/";
        private readonly IRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ProductController(IRepository<Product> productRepo,
                                IMapper mapper,
                                IUrlHelper urlHelper,
                                IConfiguration config,
                                IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _config = config;
            _urlHelper = urlHelper;
            _mapper = mapper;
            _productRepo = productRepo;
        }
        #endregion

        #region Public Api
        [HttpGet("shopping")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductForShopPage()
        {
            var productFromRepo = await _productRepo.GetAll();
            var product = _mapper.Map<IEnumerable<ProductDto>>(productFromRepo);

            foreach (var item in product)
            {
                List<string> lstImages = GetListImages(item.ProductName);
                item.ListImages = lstImages;
            }
            return Ok(product);
        }

        private List<string> GetListImages(string productName)
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, _productImagesFolder + productName);
            List<string> lstImagesName = new List<string>();
            string[] files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var test = Path.GetFileName(file);
                lstImagesName.Add(test);
            }
            return lstImagesName;
        }

        [HttpGet(Name = "GetProducts")]
        public async Task<IActionResult> GetProducts(int? pageNumber = 1)
        {
            PagingParams pagingParams = new PagingParams
            {
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
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(int id)
        {
            var productFromRepo = await _productRepo.Find(id);
            var product = _mapper.Map<ProductDto>(productFromRepo);
            List<string> lstImages = GetListImages(product.ProductName);
            product.ListImages = lstImages;
            return Ok(product);
        }

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm]CreateProductDto dataToCreate)
        {
            var productFromRepo = await _productRepo.GetSingleWithCondition(s => s.ProductName == dataToCreate.ProductName);
            if (productFromRepo != null)
                return BadRequest("Product Exists !!!");

            var imageUrl = string.Empty;
            if (Request.Form.Files.Count > 0)
            {
                imageUrl = await UploadFile(Request.Form.Files, dataToCreate.ProductName);
            }

            var product = _mapper.Map<Product>(dataToCreate);
            product.ImageUrl = imageUrl;
            product.CreatedDate = DateTime.Now;
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm]EditProductDto dataToEdit)
        {
            var oldData = await _productRepo.Find(dataToEdit.ProductId);
            var pathToDelete = Path.Combine(_hostingEnvironment.WebRootPath, oldData.ImageUrl);
            var imageUrl = oldData.ImageUrl;

            if (oldData.ProductName != dataToEdit.ProductName)
            {
                if (!dataToEdit.isAddNewFiles)
                {
                    var filesInOldFolder = Directory.GetFiles(pathToDelete);
                    if (filesInOldFolder.Count() > 0)
                    {
                        var newPath = Path.Combine(_hostingEnvironment.WebRootPath, _productImagesFolder + dataToEdit.ProductName);
                        if (!Directory.Exists(newPath))
                        {
                            System.IO.Directory.CreateDirectory(newPath);
                        }
                        foreach (var item in filesInOldFolder)
                        {
                            var sourceFile = Path.Combine(pathToDelete, Path.GetFileName(item));
                            var destFile = Path.Combine(newPath, Path.GetFileName(item));
                            System.IO.File.Copy(sourceFile, destFile, true);
                        }
                    }
                    imageUrl = _productImagesFolder + dataToEdit.ProductName;
                }
                else
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        imageUrl = await UploadFile(Request.Form.Files, dataToEdit.ProductName);
                    }
                }
                DeleteAllFileInPath(pathToDelete);
            }
            else
            {
                if (dataToEdit.isAddNewFiles)
                {
                    DeleteAllFileInPath(pathToDelete);
                    if (Request.Form.Files.Count > 0)
                    {
                        imageUrl = await UploadFile(Request.Form.Files, dataToEdit.ProductName);
                    }
                }
            }

            var product = _mapper.Map<Product>(dataToEdit);
            product.ImageUrl = imageUrl;

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
        #endregion

        #region File's Action
        private async Task<string> UploadFile(IFormFileCollection files, string productName)
        {
            string folderPath = string.Empty;
            var uploadRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, _productImagesFolder + productName);
            if (!Directory.Exists(uploadRootFolder))
            {
                Directory.CreateDirectory(uploadRootFolder);
            }
            else
            {
                var filesInOldFolder = Directory.GetFiles(uploadRootFolder);
                if (filesInOldFolder.Count() > 0)
                {
                    foreach (var item in filesInOldFolder)
                    {
                        System.IO.File.Delete(Path.Combine(uploadRootFolder, Path.GetFileName(item)));
                    }
                }
            }
            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    continue;
                }
                var filePath = Path.Combine(uploadRootFolder, file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream).ConfigureAwait(false);
                    // lstFilePathUploaded.Add(_productImagesFolder + productName + file.FileName);
                    // return file.FileName;
                }
            }
            return _productImagesFolder + productName;
        }

        private void DeleteAllFileInPath(string path)
        {
            Directory.Delete(path, true);
        }
        #endregion
    }
}