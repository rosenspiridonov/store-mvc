﻿namespace Store.Web.Models
{
    public class AddToCartRequest
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
