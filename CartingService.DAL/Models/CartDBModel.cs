using System;
using System.Collections.Generic;

namespace CartingService.DAL.Models
{
    public class CartDBModel
    {
        public Guid Id { get; set; }
        public List<CartItemDBModel> Items { get; set; } = new();
    }
}
