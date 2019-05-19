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
        public void Create(OrderModel orderVM)
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
            Product product = db.Products.Get(orderproduct.ProductId);
            order.Sum += product.ProductId * orderproduct.Amount;
            db.Orders.Create(order);
             db.Save();

        }

        public Order Get(int id)
        {
            return db.Orders.Get(id);
        }
        public IEnumerable<Order> GetAll()
        {
            return db.Orders.GetAll();
        }

        public void Edit(Order order)
        {
            db.Orders.Edit(order);
            db.Orders.Save();
        }

        public void Delete(int id)
        {
            db.Orders.Delete(id);
            db.Orders.Save();
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

    }
}
