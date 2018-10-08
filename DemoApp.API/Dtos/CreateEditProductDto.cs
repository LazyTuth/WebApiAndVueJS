using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DemoApp.API.Data;

namespace DemoApp.API.Dtos
{
    public class CreateEditProductDto
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ProductName { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        [Required]
        public string Price { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public string ProductCateCode { get; set; }
        public IEnumerable<ProductCategory> ListCate { get; set; }
    }
}