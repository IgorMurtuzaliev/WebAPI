using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public ActionResult ShowCart()
        {
            var cart = new CartIndexViewModel
            {
                Cart = GetCart(),
                
            };
            return Ok(cart.Cart.Lines.ToList());
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromForm]int productId)
        {
            var product = await productService.Get(productId);
            if (product != null)
            {
                Cart cart = GetCart();
                cart.AddItem(product, 1);
                SaveCart(cart);
            }
            return Ok(product);
        }
      
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            Product product = await productService.Get(productId);
            if (product != null)
            {
                Cart cart = GetCart();
                cart.RemoveLine(product);
                SaveCart(cart);
            }
            return Ok(product);
        }
        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }
        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }
    }
}