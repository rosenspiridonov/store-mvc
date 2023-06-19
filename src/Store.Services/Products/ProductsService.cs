using Microsoft.EntityFrameworkCore;

using Store.Commons.Extensions;
using Store.Services.Products.Models;
using Store.Data;
using Store.Data.Entities;
using Store.Commons.Exeptions;
using Store.Services.Images;

namespace Store.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public ProductsService(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<ProductModel> GetByIdAsync(int productId)
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

        public async Task<List<ProductModel>> GetRelatedProductsAsync(int productId, string categoryName, int? count)
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

        public async Task<ProductListingModel> GetFilteredProductsAsync(int pageNumber = 1, int pageSize = 12, string category = null)
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

        public async Task<int> CreateProductAsync(ProductEditModel productModel)
        {
            var product = new Product
            {
                Name = productModel.Name,
                Description = productModel.Description,
                Price = productModel.Price
            };

            foreach (var categoryId in productModel.CategoryIds)
            {
                product.ProductCategories.Add(new ProductCategory { CategoryId = categoryId });
            }

            if (productModel.Image != null)
            {
                product.ImageURL = await _imageService.SaveImage(productModel.Image, product.Name);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product.Id;
        }

        public async Task<int> UpdateProductAsync(ProductEditModel productModel)
        {
            var product = await _context.Products
                .Include(p => p.ProductCategories)
                .FirstOrDefaultAsync(p => p.Id == productModel.Id);
            
            if (product is null)
            {
                throw new UserNotificationException("Product not found");
            }

            product.Name = productModel.Name;
            product.Description = productModel.Description;
            product.Price = productModel.Price;

            product.ProductCategories.Clear();
            foreach (var categoryId in productModel.CategoryIds)
            {
                product.ProductCategories.Add(new ProductCategory { CategoryId = categoryId });
            }

            if (productModel.Image != null)
            {
                product.ImageURL = await _imageService.SaveImage(productModel.Image, product.Name);
            }

            await _context.SaveChangesAsync();
            return productModel.Id;
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product is null)
            {
                throw new UserNotificationException("Product not found");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
