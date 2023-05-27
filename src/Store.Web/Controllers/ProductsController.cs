using System.CodeDom;

using Microsoft.AspNetCore.Mvc;

using Store.Services.Products;
using Store.Services.Products.Models;

namespace Store.Web.Controllers
{
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productsService.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductDetailsModel
            {
                Product = product,
                RelatedProducts = await _productsService.GetRelatedProducts(product.Id, product.CategoryName, 4)
            };

            return View(model);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> All(int page = 1, int pageSize = 12, string category = null)
        {
            var model = await _productsService.GetFilteredProducts(page, pageSize, category);

            model.SelectedCategory = category;
            model.Categories = await _productsService.GetAllCategories();

            return View(model);
        }
    }
}
