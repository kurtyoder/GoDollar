using System;
using System.Collections.Generic;
using System.Text;

namespace GoDollarLibrary.Models
{
    public class BudgetItemModel : BaseModel
    {
        private decimal transactionTotal;
        private decimal amount;

        public int Id { get; set; }
        /// <summary>
        /// Should be the first of whatever month budget applies to
        /// </summary>
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsIncome { get; set; }
        /// <summary>
        /// Amount budged for the category for the date
        /// </summary>
        public decimal Amount { get => amount; set { amount = value; OnPropertyChanged(nameof(RemainingAmount)); } }

        /// <summary>
        /// Total amount from transactions of the same month loaded for display
        /// </summary>
        public decimal TransactionTotal { get => transactionTotal; set { transactionTotal = value; OnPropertyChanged(nameof(RemainingAmount)); } }

        public decimal RemainingAmount => Amount - TransactionTotal;
    }
}
