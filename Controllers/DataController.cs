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
        public string GetMonthlyOrderStats([FromQuery] int? year, [FromQuery] int? month)
        {
            var query = _context.MonthlyOrderStats.AsQueryable();

            if (year.HasValue)
            {
                query = query.Where(m => m.Year == year.Value);
            }

            if (month.HasValue)
            {
                query = query.Where(m => m.Month == month.Value);
            }

            var stats = query.ToList();
            return JsonSerializer.Serialize(stats);
        }
    }
}
