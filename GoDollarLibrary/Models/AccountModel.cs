using System;
using System.Collections.Generic;
using System.Text;

namespace GoDollarLibrary.Models
{
    public class AccountModel : BaseModel
    {
        private string name;
        private decimal amount;

        public int Id { get; set; }
        public string Name { get => name; set { name = value; OnPropertyChanged(nameof(Name)); } }
        public DateTime DateCreated { get; set; }
        public decimal Amount { get => amount; set { amount = value; OnPropertyChanged(nameof(Amount)); } }
    }
}
