using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SCore.BLL.Infrastructure;
using SCore.BLL.Interfaces;
using SCore.BLL.Models;
using SCore.Models;
using SCore.WEB.ViewModels;

namespace SCore.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private IProductService productService;

        public CartController(IProductService _productService)
        {
            productService = _productService;
        }
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult CartReport()
        {
            var cart = new CartIndexViewModel
            {
                Cart = GetCart(),
                
            };
            return Ok(cart);
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<ActionResult<Cart>> AddingToCart([FromForm]int id)
        {
            var product = await productService.Get(id);
            if (product != null)
            {
                Cart cart = GetCart();
                cart.AddItem(product, 1);
                SaveCart(cart);
                return Ok(cart);
            }
            return NotFound();
        }

        [Authorize(Roles = "User")]
        public async Task<ActionResult<Cart>> RemovingFromCart([FromForm]int id)
        {
            Product product = await productService.Get(id);
            if (product != null)
            {
                Cart cart = GetCart();
                cart.RemoveLine(product);
                SaveCart(cart);
                return Ok(cart);
            }
            return NotFound();
        }

        [Authorize(Roles = "User")]
        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }

        [Authorize(Roles = "User")]
        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }

    }
}