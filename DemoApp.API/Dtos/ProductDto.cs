using System.Collections.Generic;
using DemoApp.API.Data;

namespace DemoApp.API.Dtos
{
    public class ProductDto
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string ImageUrl { get; set; }

        public string ProductCateName { get; set; }
    }

    public class ProductOutputModel
    {
        public PagingHeader Paging { get; set; }
        public List<LinkInfo> Links { get; set; }
        public List<ProductDto> Items { get; set; }
    }
}