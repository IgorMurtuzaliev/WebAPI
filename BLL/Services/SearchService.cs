using ClosedXML.Excel;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using SCore.BLL.Interfaces;
using SCore.DAL.EF;
using SCore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCore.BLL.Services
{
    public class SearchService : ISearchService
    {
        private readonly UserManager<User> manager;
        private ApplicationDbContext context;
        private readonly IOrderService orderService;
        public SearchService( ApplicationDbContext _context, UserManager<User> _manager,IOrderService _orderService)
        {
            context = _context;
            manager = _manager;
            orderService = _orderService;
        }

        public async Task<IEnumerable<Order>> Search(DateTime? from, DateTime? to, string search)
        {
            if ((from != null || to != null) && search != null)
            {
                return await context.Orders.Where(p => p.User.Name.Contains(search) || p.User.LastName.Contains(search) || p.User.UserName.Contains(search)).Where(c => c.TimeOfOrder > from && c.TimeOfOrder < to).ToListAsync();
            }
            if (from != null || to != null)
            {
                return await context.Orders.Where(c => c.TimeOfOrder > from && c.TimeOfOrder < to).ToListAsync();
            }
            if (search != null)
            {
                return await context.Orders.Where(p => p.User.Name.Contains(search) || p.User.LastName.Contains(search) || p.User.UserName.Contains(search)).ToListAsync();
            }

            return await context.Orders.ToListAsync();
        }

        public async Task<XLWorkbook> ExportToExcel(DateTime? from, DateTime? to, string search)
        {
            DataTable dataTable = new DataTable("Grid");

            dataTable.Columns.AddRange(new DataColumn[4]
            {
                new DataColumn("User"),
                new DataColumn("OrderId"),
                new DataColumn("Order time"),
                 new DataColumn("Total"),
            });
            var orders = await Search(from, to, search);
            foreach (var order in orders)
            {
                dataTable.Rows.Add(order.User.UserName, order.OrderId, order.TimeOfOrder, order.Sum);
            }
            XLWorkbook wb = new XLWorkbook();
            var sheet = wb.Worksheets.Add("My Data");
            sheet.Cell(1, 1).Value = DateTime.Now;
            if (from != null && to != null)
            {
                sheet.Cell(2, 1).Value = $"Report of orders by date: with {from} to {to}";
            }
            else if (search != null)
            {

                sheet.Cell(2, 1).Value = $"Report of orders by search: {search}";
            }
            else if ((from != null && to != null && search != null))
            {
                sheet.Cell(2, 1).Value = $"Report of orders by search: {search} by date: with {from} to {to}";
            }
            else sheet.Cell(2, 1).Value = "Report of total orders";
            var tableWithData = sheet.Cell(3, 1).InsertTable(dataTable);
            sheet.Columns().AdjustToContents();
            return wb;
        }
        public async Task SendByEmail(DateTime? from, DateTime? to, string search)
        {
            var wb = await ExportToExcel(from, to, search);
            MemoryStream memoryStream = new MemoryStream();
            wb.SaveAs(memoryStream);
            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Service administration", "ingwarrior.99@yandex.ru"));
            emailMessage.To.Add(new MailboxAddress("", "ingwarrior.99@yandex.ru"));
            emailMessage.Subject = "Excel";
            var builder = new BodyBuilder();
            builder.Attachments.Add("MyGridView.xlsx", new MemoryStream(bytes));
            emailMessage.Body = builder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 25, false);
                await client.AuthenticateAsync("ingwarrior.99@yandex.ru", "038161401IngWar9991");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
