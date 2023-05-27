using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Products.Models
{
    public class ProductDetailsModel
    {
        public ProductModel Product { get; set; }

        public List<ProductModel> RelatedProducts { get; set; }
    }
}
