using SCore.DAL.EF;
using SCore.DAL.Interfaces;
using SCore.Models;
using System;
using System.Collections.Generic;

namespace SCore.DAL.Repositories
{
    class ProductOrderRepository : IRepository<ProductOrder>
    {
        private ApplicationDbContext db;
        public ProductOrderRepository(ApplicationDbContext context)
        {
            this.db = context;
        }
        public void Create(ProductOrder item)
        {
            db.ProductOrders.Add(item);
        }

        public void Delete(int? id)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Edit(ProductOrder item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductOrder> Find(Func<ProductOrder, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public ProductOrder Get(int? id)
        {
            throw new NotImplementedException();
        }

        public ProductOrder Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductOrder> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
