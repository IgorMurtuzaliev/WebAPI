using SCore.BLL.Interfaces;
using SCore.BLL.Models;
using SCore.DAL.EF;
using SCore.DAL.Interfaces;
using SCore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCore.BLL.Services
{
    public class UserService:IUserService
    {
        IUnitOfWork db { get; set; }
        private readonly IFileManager fileManager;
        private ApplicationDbContext context;
        public UserService(IUnitOfWork _db, IFileManager _fileManager,ApplicationDbContext _context)
        {
            db = _db;
            fileManager = _fileManager;
            context = _context;
        }

        public async Task Create(UserModel model)
        {
            User user = new User
            {
                Name = model.Name,
                LastName = model.LastName,
                Email = model.Email,
                Id = model.Id,
            };
            if (model.Avatar != null)
            {
                user.Avatar = fileManager.SaveImage(model.Avatar);
            }
            await db.Users.Create(user);
            await db.Users.Save();
        }

        public async Task<User> Get(int id)
        {
            return await db.Users.Get(id);
        }

     
        public async Task<IEnumerable<User>> GetAll()
        {
            return await db.Users.GetAll();
        }

        public async Task Edit(UserModel model)
        {
            User user = await Get(model.Id);
            if (user != null)
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
            await db.Users.Edit(user);
            await db.Users.Save();
        }

        public async Task<User> Get(string id)
        {
            return await db.Users.Get(id);
        }

        public async Task Delete(string id)
        {
            await db.Users.Delete(id);
            await db.Users.Save();
        }

        public void Dispose(bool disposing)
        {
            db.Dispose(disposing);
        }

        public async Task Save()
        {
            await db.Users.Save();
        }

        public bool UserExists(string id)
        {
            return context.Users.Any(e => e.Id == id);
        }
    }
}
