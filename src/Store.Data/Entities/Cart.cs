using Microsoft.AspNetCore.Identity;

namespace Store.Data.Entities
{
    public class Cart
    {
        public Cart()
        {
            Items = new HashSet<CartItem>();
        }

        public int Id { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public virtual ICollection<CartItem> Items { get; set; }
    }

}
