using SCore.BLL.Models;
using SCore.Models;
using System.Collections.Generic;

namespace SCore.BLL.Interfaces
{
    public interface IOrderService
    {
        void Create(OrderModel orderVM);
        Order Get(int id);
        void Delete(int id);
        IEnumerable<Order> GetAll();
        void Edit(Order product);
        void Dispose(bool disposing);

        IEnumerable<Order> Orders { get; }
        void SaveOrder(Order order);

    }
}
