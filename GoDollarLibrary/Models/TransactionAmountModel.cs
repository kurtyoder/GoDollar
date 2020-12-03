using System;
using System.Collections.Generic;
using System.Text;

namespace GoDollarLibrary.Models
{
    /// <summary>
    /// Used to transaction amounts for date range
    /// </summary>
    public class TransactionAmountModel
    {
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
    }
}
