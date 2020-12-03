using System;
using System.Collections.Generic;
using System.Text;

namespace GoDollarLibrary.Models
{
    public class CategoryModel : BaseModel
    {
        private bool isIncome;
        private string name;

        public int Id { get; set; }
        public string Name { get => name; set { name = value; OnPropertyChanged(nameof(Name)); } }
        public DateTime CreatedDate { get; set; }
        public bool IsIncome { get => isIncome; set { isIncome = value; OnPropertyChanged(nameof(IsIncome)); } }

    }
}
