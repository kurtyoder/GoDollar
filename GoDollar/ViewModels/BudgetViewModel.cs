using GoDollar.Helpers;
using GoDollarLibrary.Data;
using GoDollarLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace GoDollar.ViewModels
{
    public class BudgetViewModel : BaseViewModel
    {
        #region Public Properties
        private DateTime _currMonth;
        /// <summary>
        /// Current month the user is viewing
        /// </summary>
        public DateTime CurrentMonth
        {
            get => _currMonth;
            set
            {
                _currMonth = value;
                OnPropertyChanged(nameof(CurrentMonth));
                LoadBudget();
            }
        } 



        private ObservableCollection<BudgetItemModel> _budgetItems = new ObservableCollection<BudgetItemModel>();
        public ObservableCollection<BudgetItemModel> BudgetItems 
        {
            get => _budgetItems;
            set
            {
                _budgetItems = value;
                OnPropertyChanged(nameof(BudgetItems));                
            }
        }

        #endregion Public Properties

        #region Commands        
        public ICommand CreateBudgetCommand { get; set; }
        public ICommand SaveBudgetCommand { get; set; }
        public ICommand RemoveBudgetCommand { get; set; }

        #endregion Commands


        private readonly IDatabaseData _data;
        public BudgetViewModel(IDatabaseData data)
        {
            _data = data;

            //Setup relay commands
            CreateBudgetCommand = new RelayCommand<ObservableCollection<BudgetItemModel>>(() => CreateBudget(), (p) => !HasBudget(p));
            SaveBudgetCommand = new RelayCommand<ObservableCollection<BudgetItemModel>> (() => SaveBudget(), (p) => HasBudget(p));
            RemoveBudgetCommand = new RelayCommand<ObservableCollection<BudgetItemModel>>(() => RemoveBudget(), (p) => HasBudget(p));

            CurrentMonth = DateTime.Now;
        }

        public void LoadBudget()
        {
            //Clear display
            BudgetItems.Clear();
                       
            List<BudgetItemModel> budget = _data.GetBudget(CurrentMonth.Year, CurrentMonth.Month);
                      
            if((budget?.Count ?? 0) > 0)
            {
                List<TransactionAmountModel> transactions = _data.GetTransactionsAmounts(CurrentMonth.Year, CurrentMonth.Month);

                //Display budget
                foreach(BudgetItemModel b in budget.OrderBy(x=>x.CategoryName).OrderBy(x=>x.IsIncome).ToList())
                {
                    b.TransactionTotal = transactions.Where(x => x.CategoryId == b.CategoryId)?.Sum(x => x.Amount) ?? 0;                   

                    BudgetItems.Add(b);
                }
            }

           
        }


        public void CreateBudget()
        {
            List<CategoryModel> categories = _data.GetCategories();

            var budgetItems = new List<BudgetItemModel>();

            foreach(CategoryModel c in categories)
            {
                var b = new BudgetItemModel()
                {
                    CategoryId = c.Id, 
                    Date = CurrentMonth                   
                };

                budgetItems.Add(b); 

            }

            _data.CreateBudget(budgetItems);

            //Could make this more efficient, but we need id numbers
            LoadBudget();
        }


        public void SaveBudget()
        {
            _data.SaveBudget(BudgetItems.ToList());            
        }

        public bool HasBudget(ObservableCollection<BudgetItemModel> budget)
        {
            return (budget?.Count ?? 0) > 0;
        }
        public void RemoveBudget()
        {         
            _data.RemoveBudget(CurrentMonth.Year, CurrentMonth.Month);
            BudgetItems.Clear();
            OnPropertyChanged(nameof(BudgetItems));
        }
    }
}
