using logistics_visualization_demo.Data;
using logistics_visualization_demo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace logistics_visualization_demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly RecordContext _context;

        public DataController(RecordContext context)
        {
            _context = context;
        }

        [HttpGet("details")]
        public string GetOrderDetails([FromQuery] int orderId)
        {
            var orderDetails = _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .ToList();
            return JsonSerializer.Serialize(orderDetails);
        }

        [HttpGet("company")]
        public string GetCompany([FromQuery] int companyId)
        {
            var company = _context.Companies
                .FirstOrDefault(c => c.CompanyId == companyId);

            return JsonSerializer.Serialize(company);
        }

        [HttpGet("order")]
        public string GetOrder([FromQuery] int orderId)
        {
            var order = _context.Orders
                .FirstOrDefault(o => o.OrderId == orderId);

            return JsonSerializer.Serialize(order);
        }

        [HttpGet("product")]
        public string GetProduct([FromQuery] int productId)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.ProductId == productId);

            return JsonSerializer.Serialize(product);
        }

        [HttpGet("monthly")]
        public string GetMonthlyOrderStats()
        {
            // var stats = _context.MonthlyOrderStats.ToList();
            var stats = new List<MonthlyOrderStat>
            {
                new MonthlyOrderStat
                {
                    Id = "1-2024-1",
                    Year = 2024,
                    Month = 1,
                    CompanyId = 1,
                    CompanyName = "Test Company 1",
                    TotalOrders = 10,
                    TotalIncome = 1500.50m
                },
                new MonthlyOrderStat
                {
                    Id = "2-2024-2",
                    Year = 2024,
                    Month = 2,
                    CompanyId = 2,
                    CompanyName = "Test Company 2",
                    TotalOrders = 15,
                    TotalIncome = 2500.75m
                }
            };
            return JsonSerializer.Serialize(stats);
        }
    }
}
