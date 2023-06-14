using Microsoft.AspNetCore.Mvc;

using Store.Services.Categories;
using Store.Services.Products;
using Store.Services.Products.Models;

namespace Store.Web.Controllers
{
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly ICategoryService _categoryService;

        public ProductsController(IProductsService productsService, ICategoryService categoryService)
        {
            _productsService = productsService;
            _categoryService = categoryService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productsService.GetByIdAsync(id);
            
            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductDetailsModel
            {
                Product = product,
                RelatedProducts = await _productsService.GetRelatedProductsAsync(product.Id, product.CategoryName, 4)
            };

            return View(model);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> All(int page = 1, int pageSize = 12, string category = null)
        {
            var model = await _productsService.GetFilteredProductsAsync(page, pageSize, category);

            model.SelectedCategory = category;
            model.Categories = (await _categoryService.GetAllCategoriesAsync()).Select(x => x.Name).ToList();

            return View(model);
        }
    }
}
