// CatalogService.API/Controllers/CategoryController.cs

using AutoMapper;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.Dtos;
using CatalogService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CatalogService.API.Controllers
{
    /// <summary>
    /// Controller for managing categories.
    /// </summary>
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get a list of all categories.
        /// </summary>
        /// <returns>The list of categories.</returns>
        [Authorize(Policy = "buyer")]
        [HttpGet("get")]
        [ProducesResponseType(200, Type = typeof(List<Category>))]
        public async Task<ActionResult<List<Category>>> GetAllCategoriesAsync()
        {

            var currentActivity = Activity.Current;
            currentActivity?.AddEvent(new ActivityEvent(nameof(GetAllCategoriesAsync)));

           
            //LogException();
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        private static readonly ActivitySource activitySource = new ActivitySource("YourNamespace");

        private void LogException()
        {
            throw new NotImplementedException();    
        }

        [HttpGet("error")]
       
        public async Task Error()
        {
            _logger.LogInformation("info logged");
            throw new NotImplementedException();
           
        }

        /// <summary>
        /// Get a category by its ID.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <returns>The category information.</returns>
        [Authorize(Policy = "buyer")]
        [HttpGet("get/{id}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Category>> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        /// <summary>
        /// Create a new category.
        /// </summary>
        /// <param name="category">The category to create.</param>
        /// <returns>The created category.</returns>
        [Authorize(Policy = "manager")]
        [HttpPost("create")]
        [ProducesResponseType(201, Type = typeof(Category))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<ActionResult<Category>> AddCategoryAsync(CreateCategoryInputDto category)
        {
            var mappedCategory = _mapper.Map<Category>(category);
            var addedCategory = await _categoryService.AddCategoryAsync(mappedCategory);
            return Ok(category);
        }

        /// <summary>
        /// Update an existing category.
        /// </summary>
        /// <param name="category">The category to update.</param>
        /// <returns>The updated category if successful, or NotFound if the category is not found.</returns>
        [Authorize(Policy = "manager")]
        [HttpPut("update")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCategoryAsync(Category category)
        {
            var updatedCategory = await _categoryService.UpdateCategoryAsync(category);
            if (updatedCategory == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        /// <summary>
        /// Delete a category by ID.
        /// </summary>
        /// <param name="id">The category ID to delete.</param>
        /// <returns>The ID of the deleted category if successful, or NotFound if the category is not found.</returns>
        [Authorize(Policy = "manager")]
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok(id);
        }
    }
}
