using SCore.BLL.Models;
using SCore.Models;
using System.Collections.Generic;

namespace SCore.BLL.Interfaces
{
    public interface IProductService
    {
        void Create(ProductModel model);
        Product Get(int id);
        void Delete(int id);
        IEnumerable<Product> GetAll();
        void Edit(ProductModel model);
        void Dispose(bool disposing);
    }
}
