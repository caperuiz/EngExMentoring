using System.ComponentModel.DataAnnotations;

namespace CatalogService.Domain.Dtos
{

    public class CreateCategoryInputDto
    {

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; }

        // [Url(ErrorMessage = "Image must be a valid URL.")]
        public string Image { get; set; }
    }
}
