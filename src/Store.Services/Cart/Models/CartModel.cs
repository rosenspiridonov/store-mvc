using Microsoft.AspNetCore.Identity;
using Store.Data.Entities;

namespace Store.Services.Cart.Models
{
    public class CartModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public List<CartItemModel> Items { get; set; }
    }
}
