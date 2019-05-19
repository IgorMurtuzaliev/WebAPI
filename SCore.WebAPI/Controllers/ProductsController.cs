using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCore.BLL.Interfaces;
using SCore.BLL.Models;
using SCore.DAL.EF;
using SCore.Models;
using SCore.WEB.ViewModels;

namespace SCore.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService productService;

        public ProductsController(ApplicationDbContext context, IProductService _productService)
        {
            _context = context;
            productService = _productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await productService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await productService.Get(id); ;
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProduct(int id, [FromForm]ProductViewModel model)
        {
            var product = new ProductModel
            {
                ProductId = model.ProductId,
                Date = model.Date,
                Description = model.Description,
                Name = model.Name,
                Price = model.Price,
                Images = model.Images
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
