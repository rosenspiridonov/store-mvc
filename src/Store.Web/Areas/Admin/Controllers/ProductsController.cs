using Microsoft.AspNetCore.Mvc;
using Store.Services.Products.Models;
using Store.Services.Products;
using Store.Services.Categories;

using static Store.Commons.Constants;

namespace Store.Web.Areas.Admin.Controllers
{
    public class ProductsController : AdminController
    {
        private readonly IProductsService _productsService;
        private readonly ICategoryService _categoryService;

        public ProductsController(IProductsService productsService, ICategoryService categoryService)
        {
            _productsService = productsService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> All(int page = 1)
        {
            var products = await _productsService.GetFilteredProductsAsync(pageNumber: page);
            return View(products);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            if (!id.HasValue)
            {
                return View(new ProductEditModel
                {
                    Categories = categories,
                    CategoryIds = new List<int>()
                });
            }

            var product = await _productsService.GetByIdAsync(id.Value);

            if (product is null)
            {
                return NotFound("Product not found");
            }

            return View(new ProductEditModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageURL = product.ImageURL,
                CategoryName = product.CategoryName,
                Categories = categories,
                CategoryIds = await _categoryService.GetProductCategories(id.Value)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditModel model)
        {
            if (!ModelState.IsValid || model.CategoryIds == null)
            {
                if (model.CategoryIds == null)
                {
                    ModelState.AddModelError(ErrorMessagesKeys.ProductCategory, "Категорията е задължителна");
                }

                model.Categories = await _categoryService.GetAllCategoriesAsync();
                return View(model);
            }

            if (model.Id == 0)
            {
                await _productsService.CreateProductAsync(model);
            }
            else
            {
                await _productsService.UpdateProductAsync(model);
            }

            return Redirect("/admin/products/all");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _productsService.DeleteProductAsync(id);
            return Ok();
        }
    }
}
