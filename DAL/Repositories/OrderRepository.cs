using Microsoft.AspNetCore.Http;
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

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(Order item)
        {
            db.Entry(item).State = EntityState.Modified;
            await Save();
        }

        public async Task<Order> Get(int? id)
        {
            return await db.Orders.FindAsync(id);
        }

        public Task<Order> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await db.Orders.ToListAsync();
        }
        public async Task<IEnumerable<Order>> GetAll(User user)
        {
            return await db.Orders.Where(c => c.User == user).ToListAsync();
        }
        public async Task Save()
        {
            await db.SaveChangesAsync();
        }

    }
}
