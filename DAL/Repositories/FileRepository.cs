using SCore.DAL.EF;
using SCore.DAL.Interfaces;
using SCore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SCore.DAL.Repositories
{
    public class FileRepository : IRepository<FileModel>
    {
        private ApplicationDbContext db;
        public FileRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void Create(FileModel item)
        {
            db.Files.Add(item);
        }

        public void Delete(int? id)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Edit(FileModel item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FileModel> Find(Func<FileModel, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public FileModel Get(int? id)
        {
            throw new NotImplementedException();
        }

        public FileModel Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FileModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
