using Store.Data.Entities;
using Store.Data;
using Microsoft.EntityFrameworkCore;
using Store.Services.Categories.Models;

namespace Store.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryModel>> GetAllCategoriesAsync()
            => await _context.Categories
                .Select(x => new CategoryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .ToListAsync();

        public async Task<List<int>> GetProductCategories(int productId)
            => await _context.Categories
                .Where(x => x.ProductCategories.Any(pc => pc.ProductId == productId))
                .Select(x => x.Id)
                .ToListAsync();

        public async Task<string> CreateCategoryAsync(string name)
        {
            var category = new Category { Name = name };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category.Name;
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
