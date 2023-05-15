using System.ComponentModel.DataAnnotations;

namespace Store.Data.Entities
{
    public class OrderStatus
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
