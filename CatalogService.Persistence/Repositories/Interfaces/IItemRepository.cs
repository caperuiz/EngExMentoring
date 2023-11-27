using CatalogService.Domain.Entities;

namespace CatalogService.Persistence.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<List<Item>> GetAllItemsAsync(int categoryId, int page, int pageSize);
        Task<Item> GetItemByIdAsync(int id);
        Task<Item> AddItemAsync(Item item);
        Task<Item> UpdateItemAsync(Item item);
        Task<bool> DeleteItemAsync(int id);
    }

}
