using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace logistics_visualization_demo.Models
{
    public class MonthlyOrderStat
    {
        public int Year { get; set; }
        
        public int Month { get; set; }
        
        public int CompanyId { get; set; }
        
        public string CompanyName { get; set; } = string.Empty;
        
        public int TotalOrders { get; set; }
        
        public decimal TotalIncome { get; set; }
    }
}
