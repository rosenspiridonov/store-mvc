using Microsoft.EntityFrameworkCore;

using Store.Commons.Extensions;
using Store.Services.Products.Models;
using Store.Data;

namespace Store.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext _context;

        public ProductsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductModel> GetById(int productId)
            => await _context.Products
                .Where(x => x.Id == productId)
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    ImageURL = x.ImageURL,
                    CategoryName = x.ProductCategories.FirstOrDefault() == null ? string.Empty : x.ProductCategories.First().Category.Name,
                })
                .FirstOrDefaultAsync();

        public async Task<List<ProductModel>> GetRelatedProducts(int productId, string categoryName, int? count)
            => await _context.Products
                .Where(!string.IsNullOrEmpty(categoryName), x => x.ProductCategories.Any(pc => pc.Category.Name.ToLower() == categoryName.ToLower()))
                .Where(x => x.Id != productId)
                .Take(count.HasValue, count.Value)
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    ImageURL = x.ImageURL,
                    CategoryName = x.ProductCategories.FirstOrDefault() == null ? string.Empty : x.ProductCategories.First().Category.Name,
                })
                .ToListAsync();

        public async Task<ProductListingModel> GetFilteredProducts(int pageNumber = 1, int pageSize = 12, string category = null)
        {
            var productsQuery = _context.Products
                .Where(!string.IsNullOrEmpty(category), x => x.ProductCategories.Any(pc => pc.Category.Name.ToLower() == category.ToLower()))
                .AsNoTracking();

            int totalItems = productsQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (pageNumber > totalPages)
            {
                pageNumber = 1;
            }

            var products = await productsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    ImageURL = x.ImageURL,
                    CategoryName = x.ProductCategories.FirstOrDefault() == null ? string.Empty : x.ProductCategories.First().Category.Name,
                })
                .ToListAsync();

            return new ProductListingModel
            {
                Products = products,
                Pagination = new PaginationModel
                {
                    CurrentPage = pageNumber,
                    TotalPages = totalPages,
                    PageSize = pageSize
                }
            };
        }

        public async Task<List<string>> GetAllCategories()
            => await _context.Categories
                .Select(x => x.Name)
                .ToListAsync();
    }
}
