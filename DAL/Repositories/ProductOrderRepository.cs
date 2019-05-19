using SCore.DAL.EF;
using SCore.DAL.Interfaces;
using SCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCore.DAL.Repositories
{
    class ProductOrderRepository : IRepository<ProductOrder>
    {
        private ApplicationDbContext db;
        public ProductOrderRepository(ApplicationDbContext context)
        {
            this.db = context;
        }
        public async Task Create(ProductOrder item)
        {
            await db.ProductOrders.AddAsync(item);
        }

        public Task Delete(int? id)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task Edit(ProductOrder item)
        {
            throw new NotImplementedException();
        }

        //public Task<IEnumerable<ProductOrder>> Find(Func<ProductOrder, bool> predicate)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<ProductOrder> Get(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductOrder> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductOrder>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }
    }
}
