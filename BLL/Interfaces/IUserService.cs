using SCore.BLL.Models;
using SCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCore.BLL.Interfaces
{
    public interface IUserService
    {
        Task Create(UserModel user);
        Task<IEnumerable<User>> GetAll();
        Task Edit(UserModel model);
        Task<User> Get(string id);
        Task Delete(string id);
        void Dispose(bool disposing);
        Task Save();
        bool UserExists(string id);
    }
}
