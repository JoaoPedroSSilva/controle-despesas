using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseControl.Models
{
    public class ConsolidatedExpense
    {
        public string Category { get; set; }
        public string MonthYear { get; set; }
        public double TotalValue { get; set; }
        public string Display => $"{MonthYear} - {Category}: R$ {TotalValue:N2}";
    }
}
