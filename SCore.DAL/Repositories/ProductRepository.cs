using Microsoft.EntityFrameworkCore;
using SCore.DAL.EF;
using SCore.DAL.Interfaces;
using SCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCore.DAL.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private ApplicationDbContext db;
        public ProductRepository(ApplicationDbContext context)
        {
            this.db = context;
        }
        public void Create(Product item)
        {
            db.Products.Add(item);
        }

        public void Delete(int? id)
        {
            Product product = db.Products.Find(id);
            if (product != null)
                db.Products.Remove(product);
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Edit(Product item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<Product> Find(Func<Product, bool> predicate)
        {
            return db.Products.Where(predicate).ToList();
        }

        public Product Get(int? id)
        {
            return db.Products.Find(id);
        }

        public Product Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAll()
        {
            return db.Products.ToList();
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
