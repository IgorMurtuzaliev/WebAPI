using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SCore.BLL.Infrastructure;
using SCore.BLL.Interfaces;
using SCore.BLL.Models;
using SCore.DAL.EF;
using SCore.DAL.Interfaces;
using SCore.Models;
using SCore.Models.Models;
using SCore.WEB.ViewModels;

namespace SCore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly IAccountService service;
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private ApplicationDbContext db;
        public AccountController(UserManager<User> _userManager, IAccountService _service, SignInManager<User> _signInManager, IUserService _userService, ApplicationDbContext _db)
        {
            service = _service;
            userManager = _userManager;
            signInManager = _signInManager;
            userService = _userService;
            db = _db;
        }
        [HttpPost]
        [Route("register")]
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

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return NotFound();
            }
            IdentityResult result = await service.ConfirmEmail(userId, code);
            if (result.Succeeded)
                return RedirectToAction("GetProducts", "Products");
            else
                return NotFound();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm]LoginViewModel model)
        {
    
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userModel = new LoginModel
                {
                    Email = model.Email,
                    Password = model.Password

                };
               var token =  await service.LogIn(userModel);
                return Ok(new { token });
            }               
           else return BadRequest(new { message = "Username or password is incorrect" });
        }
        [Authorize]
        [HttpGet]
        [Route("account")]
        public async Task<IActionResult> UsersAccount()
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            User user = await userManager.FindByIdAsync(id);
            return Ok(user.UserName);
        }
    }
}