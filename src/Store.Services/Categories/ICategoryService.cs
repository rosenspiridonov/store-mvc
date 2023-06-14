using Microsoft.EntityFrameworkCore;
using Store.Data.Entities;
using Store.Services.Categories.Models;

namespace Store.Services.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryModel>> GetAllCategoriesAsync();

        Task<List<int>> GetProductCategories(int productId);

        Task<string> CreateCategoryAsync(string name);

        Task DeleteCategoryAsync(int categoryId);
    }
}
