using SCore.DAL.EF;
using SCore.DAL.Interfaces;
using SCore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SCore.DAL.Repositories
{
    public class FileRepository : IRepository<FileModel>
    {
        private ApplicationDbContext db;
        public FileRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public async Task Create(FileModel item)
        {
            await db.Files.AddAsync(item);
        }

        public Task Delete(int? id)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task Edit(FileModel item)
        {
            throw new NotImplementedException();
        }

        //public IEnumerable<FileModel> Find(Func<FileModel, bool> predicate)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<FileModel> Get(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<FileModel> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FileModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }
    }
}
