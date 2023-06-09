﻿namespace Store.Data.Entities
{
    public class ProductCategory
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
