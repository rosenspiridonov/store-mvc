using Store.Data.Entities;
using Store.Data;
using Microsoft.EntityFrameworkCore;
using Store.Services.Cart.Models;
using Store.Services.Products.Models;
using Store.Commons.Exeptions;

namespace Store.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CartModel> GetOrCreateCartAsync(string userId)
        {
            var cart = await GetOrCreateUserCartEntityAsync(userId);

            return ToCartModel(cart);
        }

        public async Task AddToCartAsync(string userId, int productId, int quantity)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UserNotificationException($"User ID cannot be null or whitespace {nameof(userId)}");
            }

            if (productId <= 0)
            {
                throw new UserNotificationException($"Product ID must be a positive integer {nameof(productId)}");
            }

            if (quantity <= 0)
            {
                throw new UserNotificationException($"Quantity must be a positive integer {nameof(quantity)}");
            }

            var cart = await GetOrCreateUserCartEntityAsync(userId);

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem == null)
            {
                var product = await _context.Products.FindAsync(productId) ?? throw new Exception("Product not found");

                cartItem = new CartItem { Product = product, Quantity = 0 };
                cart.Items.Add(cartItem);
            }

            cartItem.Quantity += quantity;

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(string userId, int productId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UserNotificationException($"User ID cannot be null or whitespace {nameof(userId)}");
            }

            if (productId <= 0)
            {
                throw new UserNotificationException($"Product ID must be a positive integer {nameof(productId)}");
            }

            var cart = await GetOrCreateUserCartEntityAsync(userId);

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem != null)
            {
                cart.Items.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UserNotificationException($"User ID cannot be null or whitespace {nameof(userId)}");
            }

            var cart = await GetOrCreateUserCartEntityAsync(userId);
            _context.CartItems.RemoveRange(cart.Items);

            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UserNotificationException($"User ID cannot be null or whitespace {nameof(userId)}");
            }

            var cart = await GetOrCreateUserCartEntityAsync(userId);
            return cart.Items.Sum(i => i.Product.Price * i.Quantity);
        }

        private async Task<Data.Entities.Cart> GetOrCreateUserCartEntityAsync(string userId)
        {
            var cart = await _context.Carts
                        .Include(c => c.Items)
                        .ThenInclude(i => i.Product)
                        .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Data.Entities.Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        private CartModel ToCartModel(Data.Entities.Cart cart) 
            => new CartModel
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.Items.Select(i => new CartItemModel
                {
                    Id = i.Id,
                    Quantity = i.Quantity,
                    Product = new CartProductModel
                    {
                        Id = i.Product.Id,
                        Name = i.Product.Name,
                        Price = i.Product.Price
                    }
                }).ToList()
            };
    }
}
