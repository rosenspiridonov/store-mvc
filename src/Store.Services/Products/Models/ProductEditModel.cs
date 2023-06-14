using Microsoft.AspNetCore.Http;

using Store.Services.Categories.Models;

namespace Store.Services.Products.Models
{
    public class ProductEditModel : ProductModel
    {
        public IFormFile Image { get; set; }

        public List<int> CategoryIds { get; set; }

        public List<CategoryModel> Categories { get; set; }
    }
}
