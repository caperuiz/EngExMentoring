using CatalogService.Domain.Entities;
using CatalogService.Persistence.Contexts;
using CatalogService.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Persistence.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetAllItemsAsync(int categoryId, int page, int pageSize)
        {
            var startIdx = (page - 1) * pageSize;
            return await _context.Items.Include(x => x.Category).Where(x => x.CategoryId == categoryId).Skip(startIdx).Take(pageSize).ToListAsync();
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            return await _context.Items.Where(z => z.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Item> AddItemAsync(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Item> UpdateItemAsync(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
                return false;

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
