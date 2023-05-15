using Microsoft.AspNetCore.Identity;

namespace Store.Data.Entities
{
    public class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; }

        public int StatusId { get; set; }
        public virtual OrderStatus Status { get; set; }

        public virtual IdentityUser User { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
