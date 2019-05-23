using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SCore.BLL.Interfaces;
using SCore.DAL.EF;
using SCore.Models;

namespace SCore.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private ApplicationDbContext context;
        private readonly ISearchService service;
        private readonly IOrderService orderService;
        private readonly UserManager<User> manager;
        private readonly IFileManager fileManager;
        private IHostingEnvironment _appEnvironment;
        public ReportController(ISearchService _service, IOrderService _orderService, ApplicationDbContext _context, UserManager<User> _manager, IFileManager _fileManager, IHostingEnvironment appEnvironment)
        {
            service = _service;
            orderService = _orderService;
            context = _context;
            manager = _manager;
            fileManager = _fileManager;
            _appEnvironment = appEnvironment;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Report(DateTime? from,DateTime? to, string search)
        {
            var orders = await service.Search(from, to, search);
            return Ok(orders);
        }

        [Authorize(Roles = "Admin")]
        public async Task<FileResult> ExportingToExcel(DateTime? from, DateTime? to, string search)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var wb = await service.ExportToExcel(from, to, search);
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.orenxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendingEmail(DateTime? from, DateTime? to, string search)
        {
            await service.SendByEmail(from, to, search);
            return RedirectToAction("FindData", "Home");
        }
    }
}