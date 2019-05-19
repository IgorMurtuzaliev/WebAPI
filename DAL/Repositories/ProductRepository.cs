using Microsoft.EntityFrameworkCore;
using SCore.DAL.EF;
using SCore.DAL.Interfaces;
using SCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCore.DAL.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private ApplicationDbContext db;
        public ProductRepository(ApplicationDbContext context)
        {
            this.db = context;
        }
        public async Task Create(Product item)
        {
            await db.Products.AddAsync(item);
        }

        public async Task Delete(int? id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product != null)
                db.Products.Remove(product);
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(Product item)
        {
            db.Entry(item).State = EntityState.Modified;
            await Save();
        }

        //public IEnumerable<Product> Find(Func<Product, bool> predicate)
        //{
        //    return db.Products.Where(predicate).ToList();
        //}

        public async Task<Product> Get(int? id)
        {
            return await db.Products.FindAsync(id);
        }

        public Task<Product> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await db.Products.ToListAsync();
        }

        public async Task Save()
        {
            db.SaveChanges();
        }
    }
}
