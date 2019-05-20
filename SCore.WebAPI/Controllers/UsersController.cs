using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCore.BLL.Interfaces;
using SCore.BLL.Models;
using SCore.Models;
using SCore.WEB.ViewModels;

namespace SCore.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IFileManager fileManager;
        private readonly UserManager<User> userManager;
        public UsersController(IUserService _userService, IFileManager _fileManager, UserManager<User> _userManager)
        {
            userService = _userService;
            fileManager = _fileManager;
            userManager = _userManager;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetUsers()
        {
            return Ok(await userService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            User user = await userService.Get(id); ;
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(string id, [FromForm]UserViewModel model)
        {
            var user = new UserModel
            {
               Name = model.Name,
               LastName = model.LastName,
               Email = model.Email,
               Avatar = model.Avatar,
               CurrentAvatar = model.CurrentAvatar,
               Id = model.Id
            };

            if (id != user.Id)
            {
                return BadRequest();
            }

            await userService.Edit(user);
            try
            {
                await userService.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser([FromForm]UserViewModel model)
        {
            var user = new UserModel
            {
                Name = model.Name,
                LastName = model.LastName,
                Email = model.Email,
                Id = model.Id,
                Avatar = model.Avatar
            };
            await userService.Create(user);
            return CreatedAtAction("GetProduct", new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(string id)
        {
            var user = await userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            await userService.Delete(id);

            return user;
        }

        private bool UserExists(string id)
        {
            return userService.UserExists(id);
        }
    }
}