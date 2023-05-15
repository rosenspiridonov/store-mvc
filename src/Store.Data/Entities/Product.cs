using System.ComponentModel.DataAnnotations;

namespace Store.Data.Entities
{
    public class Product
    {
        public Product()
        {
            ProductCategories = new HashSet<ProductCategory>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageURL { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
