// CatalogService.Application/Interfaces/IItemService.cs

using CatalogService.Domain.Entities;

namespace CatalogService.Application.Interfaces
{
    public interface IItemService
    {
        Task<List<Item>> GetAllItemsAsync(int categoryId, int page, int pageSize);
        Task<Item> GetItemByIdAsync(int id);
        Task<Item> AddItemAsync(Item item);
        Task<Item> UpdateItemAsync(Item item);
        Task<bool> DeleteItemAsync(int id);
    }
}
