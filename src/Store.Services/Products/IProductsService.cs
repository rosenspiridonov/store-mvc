namespace Store.Services.Products
{
    public interface IProductsService
    {
        Task<ProductPaginationModel> All(int pageNumber = 1, int pageSize = 12);
    }
}
