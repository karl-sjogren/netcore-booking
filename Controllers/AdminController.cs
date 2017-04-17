using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using WebApplication.Contracts;
using WebApplication.Models;

namespace WebApplication.Controllers {
    [Authorize]
    public class AdminController : Controller {
        private readonly ITicketService _ticketService;

        public AdminController(ITicketService ticketService) {
            _ticketService = ticketService;
        }

        public IActionResult Index(Int32 pageIndex = 0, Int32 pageSize = 20) {
            var model = new AdminIndexModel {
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            model.Tickets = _ticketService.GetOrders(pageIndex, pageSize);
            model.OrderedTickets = _ticketService.GetTicketCount();
            model.PaidTickets = _ticketService.GetPaidTicketCount();
            model.TotalPages = (Int32)Math.Ceiling((double)_ticketService.GetOrderCount() / pageSize);
            return View(model);
        }

        [HttpPut("/admin/api/orders/{orderId}/set-paid")]
        public IActionResult SetPaid(string orderId) {
            var order = _ticketService.GetOrder(orderId);
            order.Paid = true;
            order.PaidDate = DateTime.Now;
            _ticketService.Save(order);
            return new ObjectResult(order);
        }

        [HttpPut("/admin/api/orders/{orderId}/unset-paid")]
        public IActionResult SetUnpaid(string orderId) {
            var order = _ticketService.GetOrder(orderId);
            order.Paid = false;
            order.PaidDate = null;
            _ticketService.Save(order);
            return new ObjectResult(order);
        }

        [HttpDelete("/admin/api/orders/{orderId}")]
        public IActionResult RemoveOrder(string orderId) {
            var order = _ticketService.GetOrder(orderId);
            order.Deleted = true;
            _ticketService.Save(order);
            return NoContent();
        }

        [HttpGet("/admin/excel-export")]
        public IActionResult ExcelExport() {
            var orders = _ticketService.GetOrders(0, Int32.MaxValue);

            using(var ms = new MemoryStream())
			using(var package = new ExcelPackage(ms)) {
                var worksheet = package.Workbook.Worksheets.Add("Suntripfesten");
                
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Address";
                worksheet.Cells[1, 4].Value = "ZipCode";
                worksheet.Cells[1, 5].Value = "City";
                worksheet.Cells[1, 6].Value = "Phone";
                worksheet.Cells[1, 7].Value = "Email";
                worksheet.Cells[1, 8].Value = "Paid";
                worksheet.Cells[1, 9].Value = "OrderDate";
                worksheet.Cells[1, 10].Value = "TicketCount";
                worksheet.Cells[1, 10].Value = "TotalTicketPrice";

                using(var range = worksheet.Cells[1, 1, 1, 10]) 
                {
                    range.Style.Font.Bold = true;
                }

                var row = 2;
                foreach(var order in orders) {
                    worksheet.Cells[row, 1].Value = order.Id;
                    worksheet.Cells[row, 2].Value = order.Name;
                    worksheet.Cells[row, 3].Value = order.Address;
                    worksheet.Cells[row, 4].Value = order.ZipCode;
                    worksheet.Cells[row, 5].Value = order.City;
                    worksheet.Cells[row, 6].Value = order.Phone;
                    worksheet.Cells[row, 7].Value = order.Email;
                    worksheet.Cells[row, 8].Value = order.Paid ? "Ja" : "Nej";
                    worksheet.Cells[row, 9].Value = order.OrderDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 10].Value = order.TicketCount;
                    worksheet.Cells[row, 11].Value = order.TicketCount * 495;

                    row++;
                }

                package.Workbook.Properties.Title = "Biljettexport för Suntripfesten";
                package.Workbook.Properties.Author = "Suntripfesten";
                package.Workbook.Properties.Comments = "Exporterat " + DateTime.Now.ToString();

                package.Save();

                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Suntripfesten.xlsx");
            }
        }
    }
}
