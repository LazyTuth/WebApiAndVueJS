using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoApp.API.Data
{
    public class ProductCategory
    {
        [Key]
        public string CateCode { get; set; }
        public string Description { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }
    }
}