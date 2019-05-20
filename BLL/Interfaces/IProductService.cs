using SCore.BLL.Models;
using SCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCore.BLL.Interfaces
{
    public interface IProductService
    {
        Task Create(ProductModel model);
        Task<Product> Get(int id);
        Task Delete(int id);
        Task<IEnumerable<Product>> GetAll();
        Task Edit(ProductModel model);
        void Dispose(bool disposing);
        Task Save();
        bool ProductExists(int id);
    }
}
