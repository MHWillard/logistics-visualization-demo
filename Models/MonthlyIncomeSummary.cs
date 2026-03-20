using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace logistics_visualization_demo.Models
{
    public class MonthlyIncomeSummary
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        
        public int Year { get; set; }
        
        public int Month { get; set; }
        
        public decimal TotalIncome { get; set; }
    }
}
