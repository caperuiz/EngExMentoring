using System;
using System.Collections.Generic;

namespace CartingService.BLL.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public List<CartItem> Items { get; set; } = new();
    }
}
