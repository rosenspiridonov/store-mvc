﻿namespace Store.Services.Products.Models
{
    public class ProductListingModel
    {
        public List<ProductModel> Products { get; set; }

        public PaginationModel Pagination { get; set; }

        public string SelectedCategory { get; set; }

        public List<string> Categories { get; set; }
    }
}
