using Store.Services.Products.Models;

namespace Store.Services.Cart.Models
{
    public class CartItemModel
    {
        public int Id { get; set; }

        public CartProductModel Product { get; set; }

        public int Quantity { get; set; }
    }
}