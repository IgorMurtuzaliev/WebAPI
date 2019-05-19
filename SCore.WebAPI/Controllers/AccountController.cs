using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SCore.BLL.Interfaces;
using SCore.BLL.Models;
using SCore.Models;
using SCore.WEB.ViewModels;

namespace SCore.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly IAccountService service;
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        public AccountController(UserManager<User> _userManager, IAccountService _service, SignInManager<User> _signInManager, IUserService _userService)
        {
            service = _service;
            userManager = _userManager;
            signInManager = _signInManager;
            userService = _userService;
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var register = new RegisterModel
                {
                    Name = model.Name,
                    LastName = model.LastName,
                    Avatar = model.Avatar,
                    CurrentAvatar = model.CurrentAvatar,
                    Email = model.Email,
                    Password = model.Password,
                    PasswordConfirm = model.PasswordConfirm
                };
                IdentityResult result = await service.Create(register, HttpContext.Request.Host.ToString());
                if (result.Succeeded)
                {
                    return Ok(register);
                }
            }
            return Ok(model);
        }
    }
}