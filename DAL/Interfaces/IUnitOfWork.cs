using SCore.Models;
using SCore.Models.Entities;

namespace SCore.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<Product> Products { get; }
        IRepository<Order> Orders { get; }
        IRepository<FileModel> Files { get; }
        void Save();
        void Dispose(bool disposing);
    }
}
