using SCore.BLL.Models;
using SCore.Models;
using System.Collections.Generic;

namespace SCore.BLL.Interfaces
{
    public interface IUserService
    {
        void Create(User user);
        IEnumerable<User> GetAll();
        void Edit(UserModel model);
        User Get(string id);
        void Delete(string id);
        void Dispose(bool disposing);
    }
}
