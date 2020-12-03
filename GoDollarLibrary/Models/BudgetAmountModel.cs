using System;
using System.Collections.Generic;
using System.Text;

namespace GoDollarLibrary.Models
{
    /// <summary>
    /// Used to get how much was budgeted for time range
    /// </summary>
    public class BudgetAmountModel
    {
        public bool IsIncome { get; set; }
        public decimal Amount { get; set; }
    }
}
