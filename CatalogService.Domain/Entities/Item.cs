// CatalogService.Domain/Entities/Item.cs

namespace CatalogService.Domain.Entities
{
    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public decimal Price { get; set; }

        public int Amount { get; set; }
    }
    public class InputItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public int Amount { get; set; }


    }

    public static class ItemExtensions
    {
        public static Item ToItem(this InputItemDto product)
        {
            return new Item
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                Price = product.Price,
                Amount = product.Amount
            };
        }
    }

}


