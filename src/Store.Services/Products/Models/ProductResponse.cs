using System.Text.Json.Serialization;

namespace Store.Services.Products.Models
{
    public class ProductResponse
    {
        [JsonPropertyName("products")]
        public List<ProductDto> Products { get; set; }
    }

    public class ProductDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }


        [JsonPropertyName("title")]
        public string Title { get; set; }


        [JsonPropertyName("description")]
        public string Description { get; set; }


        [JsonPropertyName("price")]
        public decimal Price { get; set; }


        [JsonPropertyName("images")]
        public List<string> Images { get; set; }


        [JsonPropertyName("category")]
        public string Category { get; set; }
    }
}
