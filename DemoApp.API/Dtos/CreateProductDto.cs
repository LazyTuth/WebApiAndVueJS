using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DemoApp.API.Dtos
{
    public class CreateProductDto
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ProductName { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Price must be numeric")]
        public string Price { get; set; }

        //public string ImageUrl { get; set; }

        [Required]
        public string ProductCateCode { get; set; }
        
        //public DateTime CreatedDate { get; set; }

        // public CreateProductDto()
        // {
        //     this.CreatedDate = DateTime.Now;
        // }
    }
}