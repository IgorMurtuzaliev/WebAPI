using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SCore.BLL.Interfaces;
using SCore.BLL.Models;
using SCore.DAL.EF;
using SCore.DAL.Interfaces;
using SCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCore.BLL.Services
{
    public class OrderService : IOrderService
    {
        IUnitOfWork db { get; set; }
        private UserManager<User> userManager;
        private ApplicationDbContext context;
        public OrderService(IUnitOfWork _db, UserManager<User> _userManager, ApplicationDbContext _context)
        {
            db = _db;
            userManager = _userManager;
            context = _context;
        }
        public async Task Create(OrderModel orderVM)
        {
            var order = new Order
            {
                UserId = orderVM.UserId,
                TimeOfOrder = DateTime.Now,
                Sum=0
            };

            var orderproduct = new ProductOrder
            {
                ProductId = orderVM.ProductId,
                Amount = orderVM.Amount,
            };
            order.ProductOrders.Add(orderproduct);
            Product product = await db.Products.Get(orderproduct.ProductId);
            order.Sum += product.Price * orderproduct.Amount;
            await db.Orders.Create(order);
             await db.Save();

        }

        public async Task<Order> Get(int id)
        {
            return await db.Orders.Get(id);
        }
        public async Task<IEnumerable<Order>> GetAll()
        {
            return await db.Orders.GetAll();
        }

        public async Task Edit(Order order)
        {
            await db.Orders.Edit(order);
            await db.Orders.Save();
        }

        public async Task Delete(int id)
        {
            await db.Orders.Delete(id);
            await db.Orders.Save();
        }

        public void Dispose(bool disposing)
        {
            db.Dispose(disposing);
        }
        public IEnumerable<Order> Orders => context.Orders
                            .Include(o => o.ProductOrders)
                            .ThenInclude(l => l.Product);

        public void SaveOrder(Order order)
        {
            context.AttachRange(order.ProductOrders.Select(l => l.Product));
            if (order.OrderId == 0)
            {
                db.Orders.Create(order);
                db.Save();
            }           
        }
        public async Task Save()
        {
            await db.Save();
        }

        public bool OrderExists(int id)
        {
            return context.Orders.Any(c => c.OrderId == id);
        }
    }
}
