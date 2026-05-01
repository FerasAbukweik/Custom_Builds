using System;
using System.Collections.Generic;
using System.Text;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
