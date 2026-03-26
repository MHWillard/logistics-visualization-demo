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
            var stats = _context.MonthlyOrderStats.ToList();
            return JsonSerializer.Serialize(stats);
        }

        [HttpGet("monthly-income-summary")]
        public string GetMonthlyIncomeSummary()
        {
            var summary = new List<MonthlyIncomeSummary>
            {
                new MonthlyIncomeSummary
                {
                    Id = "2025-1",
                    Year = 2025,
                    Month = 1,
                    TotalIncome = 12500.00m
                },
                new MonthlyIncomeSummary
                {
                    Id = "2025-2",
                    Year = 2025,
                    Month = 2,
                    TotalIncome = 11200.50m
                },
                new MonthlyIncomeSummary
                {
                    Id = "2025-3",
                    Year = 2025,
                    Month = 3,
                    TotalIncome = 15800.75m
                },
                new MonthlyIncomeSummary
                {
                    Id = "2025-4",
                    Year = 2025,
                    Month = 4,
                    TotalIncome = 14300.25m
                },
                new MonthlyIncomeSummary
                {
                    Id = "2025-5",
                    Year = 2025,
                    Month = 5,
                    TotalIncome = 16500.00m
                },
                new MonthlyIncomeSummary
                {
                    Id = "2025-6",
                    Year = 2025,
                    Month = 6,
                    TotalIncome = 18200.50m
                },
                new MonthlyIncomeSummary
                {
                    Id = "2025-7",
                    Year = 2025,
                    Month = 7,
                    TotalIncome = 19500.25m
                },
                new MonthlyIncomeSummary
                {
                    Id = "2025-8",
                    Year = 2025,
                    Month = 8,
                    TotalIncome = 21000.00m
                },
                new MonthlyIncomeSummary
                {
                    Id = "2025-9",
                    Year = 2025,
                    Month = 9,
                    TotalIncome = 17800.75m
                },
                new MonthlyIncomeSummary
                {
                    Id = "2025-10",
                    Year = 2025,
                    Month = 10,
                    TotalIncome = 16200.50m
                },
                new MonthlyIncomeSummary
                {
                    Id = "2025-11",
                    Year = 2025,
                    Month = 11,
                    TotalIncome = 14500.25m
                },
                new MonthlyIncomeSummary
                {
                    Id = "2025-12",
                    Year = 2025,
                    Month = 12,
                    TotalIncome = 13800.00m
                }
            };
            return JsonSerializer.Serialize(summary);
        }

        [HttpGet("orders")]
        public string GetAllOrders()
        {
            var orders = _context.Orders.ToList();
            return JsonSerializer.Serialize(orders);
        }
    }
}
