using AutoMapper;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.Dtos;
using CatalogService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers
{

    [Route("api/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Controller for managing items.
        /// </summary>
        public ItemController(IItemService itemService, IMapper mapper)
        {
            _itemService = itemService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of all items.
        /// </summary>
        /// <param name="categoryId">The category ID.</param>
        /// <param name="page">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The list of items.</returns>
        [HttpGet("get")]
        [ProducesResponseType(200, Type = typeof(List<Item>))]
        public async Task<ActionResult<List<Item>>> GetAllItemsAsync(int categoryId, int page, int pageSize)
        {
            var items = await _itemService.GetAllItemsAsync(categoryId, page, pageSize);
            return Ok(items);
        }

        /// <summary>
        /// Get an item by ID.
        /// </summary>
        /// <param name="id">The item ID.</param>
        /// <returns>The item information.</returns>
        [HttpGet("get/{id}")]
        [ProducesResponseType(200, Type = typeof(Item))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Item>> GetItemByIdAsync(int id)
        {
            throw new NotImplementedException();
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        /// <summary>
        /// Create a new item.
        /// </summary>
        /// <param name="item">The item to create.</param>
        /// <returns>The created item.</returns>
        [HttpPost("create")]
        [ProducesResponseType(201, Type = typeof(Item))]
        [ProducesResponseType(400, Type = typeof(string))]

        public async Task<ActionResult<Item>> AddItemAsync(CreateItemInputDto item)
        {
            var mappedItem = _mapper.Map<Item>(item);
            var addedItem = await _itemService.AddItemAsync(mappedItem);
            return Ok(addedItem);
        }

        /// <summary>
        /// Update an existing item.
        /// </summary>
        /// <param name="item">The item to update.</param>
        /// <returns>No content if successful, or NotFound if the item is not found.</returns>
        [HttpPut("update")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public async Task<IActionResult> UpdateItemAsync(InputItemDto item)
        {
            var updatedItem = await _itemService.UpdateItemAsync(item.ToItem());
            if (updatedItem == null)
            {
                return NotFound();
            }

            return Ok(updatedItem);
        }

        /// <summary>
        /// Delete an item by ID.
        /// </summary>
        /// <param name="id">The item ID to delete.</param>
        /// <returns>No content if successful, or NotFound if the item is not found.</returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteItemAsync(int id)
        {
            var result = await _itemService.DeleteItemAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }

        private Item MapCreateItemInputDtoToItem(CreateItemInputDto inputDto)
        {
            return new Item
            {
                Name = inputDto.Name,
                Description = inputDto.Description,
                ImageUrl = inputDto.ImageUrl,
                CategoryId = inputDto.CategoryId,
                Price = inputDto.Price,
                Amount = inputDto.Amount
            };
        }

    }
}
