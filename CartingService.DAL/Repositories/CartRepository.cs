using CartingService.DAL.Models;
using System;
using System.Collections.Generic;

namespace CartingService.DAL.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly LiteDBContext _context;

        public CartRepository(LiteDBContext context)
        {
            _context = context;
        }

        public CartDBModel GetById(Guid id)
        {
            return _context.Carts.FindOne(c => c.Id == id);
        }

        public void Update(CartDBModel cart)
        {
            _context.Carts.Update(cart);
        }

        public void Insert(CartDBModel cart)
        {
            _context.Carts.Insert(cart);
        }

        public void Delete(Guid id)
        {
            _context.Carts.Delete(id);
        }

        public IEnumerable<CartDBModel> GetAll()
        {
            return _context.Carts.FindAll();
        }
    }
}
