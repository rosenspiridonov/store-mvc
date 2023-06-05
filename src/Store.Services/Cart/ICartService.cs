using Store.Services.Cart.Models;

namespace Store.Services.Cart
{
    public interface ICartService
    {
        Task<CartModel> GetOrCreateCartAsync(string userId);

        Task<decimal> GetTotalAsync(string userId);

        Task AddToCartAsync(string userId, int productId, int quantity);

        Task RemoveFromCartAsync(string userId, int productId);

        Task ClearCartAsync(string userId);
    }
}
