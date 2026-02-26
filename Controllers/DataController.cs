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

        [HttpGet]
        public string GetRecords([FromQuery] int orderId)
        {
            var orderDetails = _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .ToList();
            return JsonSerializer.Serialize(orderDetails);
        }
    }
}
