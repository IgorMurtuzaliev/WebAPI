using SCore.BLL.Models;
using SCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCore.BLL.Interfaces
{
    public interface IOrderService
    {
        Task Create(OrderModel orderVM);
        Task<Order> Get(int id);
        Task Delete(int id);
        Task<IEnumerable<Order>> GetAll(User user);
        Task Edit(Order product);
        void Dispose(bool disposing);

        IEnumerable<Order> Orders { get; }
        Task SaveOrder(Order order);
        Task Save();
        bool OrderExists(int id);


    }
}
