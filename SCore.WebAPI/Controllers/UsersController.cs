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
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return Ok(await userService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await userService.Get(id); ;
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProduct(int id, [FromForm]UserViewModel model)
        {
            var user = new UserModel
            {

                Name = model.Name,
                LastName = model.LastName,
                Email = model.Email,
                CurrentAvatar = model.Avatar,
                
            };

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            await productService.Edit(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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
        public async Task<ActionResult<Product>> AddProduct([FromForm]ProductViewModel model)
        {
            var product = new ProductModel
            {
                Name = model.Name,
                Price = model.Price,
                Date = model.Date,
                Description = model.Description,
                ProductId = model.ProductId,
                Images = model.Images
            };
            await productService.Create(product);
            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await productService.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            await productService.Delete(id);

            return product;
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}