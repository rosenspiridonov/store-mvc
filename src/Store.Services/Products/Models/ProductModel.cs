using System.ComponentModel.DataAnnotations;

namespace Store.Services.Products.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името на продукта е задължително поле")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Описанието на продукта е задължително поле")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Цената на продукта е задължително поле")]
        public decimal Price { get; set; }

        public string ImageURL { get; set; }

        public string CategoryName { get; set; }
    }
}
