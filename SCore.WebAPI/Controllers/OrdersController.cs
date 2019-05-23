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
using SCore.DAL.EF;
using SCore.Models;
using SCore.WEB.ViewModels;

namespace SCore.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService service;
        private Cart cart;
        private UserManager<User> userManager;
        public OrdersController(IOrderService _service, Cart _cart, UserManager<User> _userManager)
        {
            service = _service;
            cart = _cart;
            userManager = _userManager;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<Order>>> OrdersList()
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            User user = await userManager.FindByIdAsync(id);
            return Ok(await service.GetAll(user));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Order>> Order(int id)
        {
            var order = await service.Get(id);
            if (order == null)
            {
                return NotFound("Order not found");
            }
            return order;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> EditingOrder(int id, [FromForm]Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }
            await service.Edit(order);
            try
            {
                await service.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound("Order not found");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Order>> CreatingOrder([FromForm]OrderViewModel model)
        {
            var order = new OrderModel
            {
                Amount = model.Amount,
                OrderId = model.OrderId,
                ProductId = model.ProductId,
                TimeOfOrder = DateTime.Now,
                UserId = model.UserId
            };
            await service.Create(order);
            await service.Save();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Order>> DeletingOrder(int id)
        {
            var order = await service.Get(id);
            if (order == null)
            {
                return NotFound("Order not found");
            }
            await service.Delete(id);
            return order;
        }

        [Authorize(Roles = "User")]
        private bool OrderExists(int id)
        {
            return service.OrderExists(id);
        }
        [Authorize]
        public async Task<IActionResult> Checkout([FromForm]Order order)
        {          
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                var id = User.Claims.First(c => c.Type == "Id").Value;
                order.UserId = id;
                User user = await userManager.FindByIdAsync(id);
                order.ProductOrders = cart.Lines.ToArray();
                order.Sum = 0;
                foreach (var line in cart.Lines)
                {
                    order.Sum += line.Product.Price * line.Amount;
                }
                await service.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return BadRequest();
            }
        }
        public IActionResult Completed()
        {
            cart.Clear();
            return Ok(cart);
        }

    }
}
