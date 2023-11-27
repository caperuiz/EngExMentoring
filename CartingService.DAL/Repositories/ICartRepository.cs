using CartingService.DAL.Models;
using System;
using System.Collections.Generic;

namespace CartingService.DAL.Repositories
{
    public interface ICartRepository
    {

        public CartDBModel GetById(Guid id);

        public void Update(CartDBModel cart);

        public void Insert(CartDBModel cart);

        public void Delete(Guid id);

        public IEnumerable<CartDBModel> GetAll();
    }
}