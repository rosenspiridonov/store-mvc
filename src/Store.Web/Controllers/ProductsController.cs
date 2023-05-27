using System.CodeDom;

using Microsoft.AspNetCore.Mvc;

using Store.Services.Products;

namespace Store.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        public async Task<IActionResult> All(int page = 1, int pageSize = 12, string category = null)
        {
            var model = await _productsService.GetFilteredProducts(page, pageSize, category);
            model.SelectedCategory = category;
            model.Categories = await _productsService.GetAllCategories();

            return View(model);
        }
    }
}
