using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCore.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int? id);
        //IEnumerable<T> Find(Func<T, Boolean> predicate);
        Task Create(T item);
        Task Edit(T item);
        Task Delete(int? id);
        Task Save();
        Task Delete(string id);
        Task<T> Get(string id);
    }
}
