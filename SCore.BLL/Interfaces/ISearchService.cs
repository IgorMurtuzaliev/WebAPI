using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SCore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SCore.BLL.Interfaces
{
    public interface ISearchService
    {
        List<Order> FindByDate(DateTime? from, DateTime? to);
        List<Order> FindByUser(string search);
        XLWorkbook ExportToExcel(DateTime? from, DateTime? to, string search);
        List<Order> Search(DateTime? from, DateTime? to, string search);
        Task SendByEmail(DateTime? from, DateTime? to, string search);
    }
}
