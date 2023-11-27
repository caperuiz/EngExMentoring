namespace CatalogService.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // [Url(ErrorMessage = "Image must be a valid URL.")]
        public string Image { get; set; }
    }
}