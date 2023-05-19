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

        public async Task<IActionResult> All(int page = 1, int pageSize = 12)
        {
            var products = await _productsService.All(page, pageSize);

            return View(products);
        }
    }
}
