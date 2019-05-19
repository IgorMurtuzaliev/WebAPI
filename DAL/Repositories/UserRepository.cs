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
    public class UserRepository : IRepository<User>
    {
        private ApplicationDbContext db;
        public UserRepository(ApplicationDbContext context)
        {
            db = context;
        }
        public async Task Create(User item)
        {
           await db.Users.AddAsync(item);
        }

        public Task Delete(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id)
        {
            User user = await db.Users.FindAsync(id);
            if (user != null)
                db.Users.Remove(user);
        }

        public async Task Edit(User item)
        {
            db.Entry(item).State = EntityState.Modified;
            await Save();
        }

        //public IEnumerable<User> Find(Func<User, bool> predicate)
        //{
        //    return db.Users.Where(predicate).ToList();
        //}

        public async Task<User> Get(string id)
        {
            return await db.Users.FindAsync(id);
        }

        public Task<User> Get(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await db.Users.ToListAsync();
        }

        public async Task Save()
        {
            db.SaveChanges();
        }

    }
}
