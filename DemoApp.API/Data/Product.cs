using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoApp.API.Data
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsHotProduct { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("ProductCategory")]
        public string ProductCateCode { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }
}