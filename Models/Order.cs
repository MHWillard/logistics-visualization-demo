using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace logistics_visualization_demo.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CompanyId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
