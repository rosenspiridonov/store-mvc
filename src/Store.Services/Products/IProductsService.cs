using Store.Services.Products.Models;

namespace Store.Services.Products
{
    public interface IProductsService
    {
        Task<ProductModel> GetByIdAsync(int productId);

        Task<List<ProductModel>> GetRelatedProductsAsync(int productId, string categoryName, int? count);

        Task<ProductListingModel> GetFilteredProductsAsync(int pageNumber = 1, int pageSize = 12, string category = null);

        Task<ProductEditModel> CreateProductAsync(ProductEditModel productModel);

        Task<int> UpdateProductAsync(ProductEditModel productModel);

        Task DeleteProductAsync(int productId);
    }
}
