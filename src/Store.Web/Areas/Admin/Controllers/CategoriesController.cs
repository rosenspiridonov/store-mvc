using Microsoft.AspNetCore.Mvc;
using Store.Services.Categories;

namespace Store.Web.Areas.Admin.Controllers
{
    public class CategoriesController : AdminController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string name)
        {
            var result = await _categoryService.CreateCategoryAsync(name);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return Ok();
        }
    }
}
