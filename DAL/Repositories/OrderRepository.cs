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
    public class OrderRepository: IRepository<Order>
    {
        private ApplicationDbContext db;
        public OrderRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public async Task Create(Order item)
        {
            await db.Orders.AddAsync(item);
        }

        public async Task Delete(int? id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order != null)
            {
                foreach(var item in order.ProductOrders)
                {
                db.ProductOrders.Remove(item);
                }

                db.Orders.Remove(order);
            }

        }

        public async Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(Order item)
        {
            db.Entry(item).State = EntityState.Modified;
            await Save();
        }

        //public async Task<IEnumerable<Order>> Find(Func<Order, bool> predicate)
        //{
        //    return db.Orders.Where(predicate).ToList();
        //}

        public async Task<Order> Get(int? id)
        {
            return await db.Orders.FindAsync(id);
        }

        public async Task<Order> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await db.Orders.Include(c=>c.User).ToListAsync();
        }

        public async Task Save()
        {
            db.SaveChanges();
        }

    }
}
