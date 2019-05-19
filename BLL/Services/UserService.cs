using SCore.BLL.Interfaces;
using SCore.BLL.Models;
using SCore.DAL.Interfaces;
using SCore.Models;
using System.Collections.Generic;

namespace SCore.BLL.Services
{
    public class UserService:IUserService
    {
        IUnitOfWork db { get; set; }
        private readonly IFileManager fileManager;
        public UserService(IUnitOfWork _db, IFileManager _fileManager)
        {
            db = _db;
            fileManager = _fileManager;
        }

        public void Create(User user)
        {
            db.Users.Create(user);
            db.Users.Save();
        }

        public User Get(int id)
        {
            return db.Users.Get(id);
        }

     
        public IEnumerable<User> GetAll()
        {
            return db.Users.GetAll();
        }

        public void Edit(UserModel model)
        {
            var user = Get(model.Id);
            if (user!= null)
            {
                user.Name = model.Name;
            user.LastName = model.LastName;
            user.Email = model.Email;
            }
            if (model.Avatar == null) user.Avatar = model.CurrentAvatar;
            if (model.Avatar != null)
            {
                user.Avatar = fileManager.SaveImage(model.Avatar);
            }
            db.Users.Edit(user);
            db.Users.Save();
        }

        public User Get(string id)
        {
            return db.Users.Get(id);
        }

        public void Delete(string id)
        {
            db.Users.Delete(id);
            db.Users.Save();
        }

        public void Dispose(bool disposing)
        {
            db.Dispose(disposing);
        }
    }
}
