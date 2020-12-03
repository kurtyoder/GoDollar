using System;
using System.Collections.Generic;
using System.Text;

namespace GoDollarLibrary.Models
{
    public class TransactionModel : BaseModel
    {
        private bool isIncome;
        private int categoryId;
        private int accountId;
        private decimal amount;
        private string note;
        private DateTime date;

        public int Id { get; set; }
        public bool IsIncome { get => isIncome; set { isIncome = value; OnPropertyChanged(nameof(IsIncome)); } }
        public int CategoryId { get => categoryId; set { categoryId = value; OnPropertyChanged(nameof(CategoryId)); } }
        public int AccountId { get => accountId; set { accountId = value; OnPropertyChanged(nameof(AccountId)); } }
        public decimal Amount { get => amount; set { amount = value; OnPropertyChanged(nameof(Amount)); } }
        public string Note { get => note; set { note = value; OnPropertyChanged(nameof(Note)); } }
        public DateTime Date { get => date; set { date = value; OnPropertyChanged(nameof(Date)); } }

        //Just for displaying

        public string Category { get; set; }
        public string Account { get; set; }
    }
}
