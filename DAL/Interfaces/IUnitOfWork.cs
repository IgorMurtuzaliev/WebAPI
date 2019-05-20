using SCore.Models;
using SCore.Models.Entities;
using System.Threading.Tasks;

namespace SCore.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<Product> Products { get; }
        IRepository<Order> Orders { get; }
        IRepository<FileModel> Files { get; }
        Task Save();
        void Dispose(bool disposing);
    }
}
