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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService service;

        public OrdersController(IOrderService _service)
        {
            service = _service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return Ok(await service.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await service.Get(id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditOrder(int id, [FromForm]Order order)
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
        public async Task<ActionResult<Order>> CreateOrder([FromForm]OrderViewModel model)
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

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await service.Get(id);
            if (order == null)
            {
                return NotFound();
            }
            await service.Delete(id);
            return order;
        }

        private bool OrderExists(int id)
        {
            return service.OrderExists(id);
        }
    }
}
