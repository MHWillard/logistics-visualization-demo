using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace logistics_visualization_demo.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
