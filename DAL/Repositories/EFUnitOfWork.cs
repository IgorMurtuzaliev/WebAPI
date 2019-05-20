using SCore.DAL.EF;
using SCore.DAL.Interfaces;
using SCore.Models;
using SCore.Models.Entities;
using System;
using System.Threading.Tasks;

namespace SCore.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db;
        private UserRepository userRepository;
        private OrderRepository orderRepository;
        private ProductRepository productRepository;
        private ProductOrderRepository productOrderRepository;
        private FileRepository fileRepository;

        public EFUnitOfWork(ApplicationDbContext context)
        {
            db = context;
        }
        public IRepository<User> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }

        public IRepository<Order> Orders
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepository(db);
                return orderRepository;
            }
        }

        public IRepository<Product> Products
        {
            get
            {
                if (productRepository == null)
                    productRepository = new ProductRepository(db);
                return productRepository;
            }
        }
        public IRepository<ProductOrder> ProductsOrders
        {
            get
            {
                if (productOrderRepository == null)
                    productOrderRepository = new ProductOrderRepository(db);
                return productOrderRepository;
            }
        }
        public IRepository<FileModel> Files
        {
            get
            {
                if (fileRepository == null)
                    fileRepository = new FileRepository(db);
                return fileRepository;
            }
        }
        public async Task Save()
        {
            await db.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
