using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/[controller]")]
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
        [Authorize(Roles = "Admin")]
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
                return NotFound("User's not found");
            }
            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromForm]UserViewModel model)
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
                    return NotFound("User's not found");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> Delete(string id)
        {
            var user = await userService.Get(id);
            if (user == null)
            {
                return NotFound("User's not found");
            }
            await userService.Delete(id);

            return user;
        }

        private bool UserExists(string id)
        {
            return userService.UserExists(id);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("user_to_manager")]
        public async Task UserToManager([FromForm]string id)
        {
            await userService.UserToManager(id);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("manager_to_user")]
        public async Task ManagerToUser(string id)
        {
            await userService.ManagerToUser(id);
        }
    }
}