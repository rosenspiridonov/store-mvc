﻿using Store.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Store.Services.Products
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageURL { get; set; }

        public string CategoryName { get; set; }
    }
}