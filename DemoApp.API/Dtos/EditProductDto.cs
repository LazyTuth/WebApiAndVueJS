using System.ComponentModel.DataAnnotations;

namespace DemoApp.API.Dtos
{
    public class EditProductDto
    {
        public int ProductId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ProductName { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Price must be numeric")]
        public string Price { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public string ProductCateCode { get; set; }
    }
}