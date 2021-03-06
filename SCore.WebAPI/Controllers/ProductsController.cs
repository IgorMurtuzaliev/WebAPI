﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private ApplicationDbContext _context;

        public ProductsController(IProductService _productService, ApplicationDbContext context)
        {
            productService = _productService;
            _context = context;
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
                return NotFound("Products's not found");
            }
            return product;
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm]ProductViewModel model)
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
                    return NotFound("Products's not found");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromForm]ProductViewModel model)
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
            const int lengthMax = 2097152;
            const string correctType = "image/jpeg";
            foreach (var image in model.Images)
            {
                var type = image.ContentType;
                var length = image.Length;
                if (type != correctType)
                {
                    ModelState.AddModelError("Uploads", "Error, allowed image resolution jpg / jpeg");
                    return BadRequest(ModelState);
                }

                if (length < lengthMax) continue;
                ModelState.AddModelError("Uploads", "Error, image size should not be more than 2 MB");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await productService.Create(product);
            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> Delete(int id)
        {
            var product = await productService.Get(id);
            if (product == null)
            {
                return NotFound("Products's not found");
            }
            await productService.Delete(id);

            return product;
        }

        private bool ProductExists(int id)
        {
            return productService.ProductExists(id);
        }
    }
}
