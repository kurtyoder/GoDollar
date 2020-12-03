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
    public class TransactionsViewModel : BaseViewModel
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
                LoadTransactions();
            }
        }

        private decimal _monthlyExpense;
        public decimal MonthlyExpense
        {
            get => _monthlyExpense;
            set { _monthlyExpense = value; OnPropertyChanged(nameof(MonthlyExpense)); OnPropertyChanged(nameof(RemainingExpense)); }
        }

        private decimal _monthlyIncome;
        public decimal MonthlyIncome
        {
            get => _monthlyIncome;
            set { _monthlyIncome = value; OnPropertyChanged(nameof(MonthlyIncome)); OnPropertyChanged(nameof(RemainingIncome)); }
        }


        private decimal _monthlyExpenseBudget;
        public decimal MonthlyExpenseBudget
        {
            get => _monthlyExpenseBudget;
            set { _monthlyExpenseBudget = value; OnPropertyChanged(nameof(MonthlyExpenseBudget)); OnPropertyChanged(nameof(RemainingExpense)); }
        }

        private decimal _monthlyIncomeBudget;
        public decimal MonthlyIncomeBudget
        {
            get => _monthlyIncomeBudget;
            set { _monthlyIncomeBudget = value; OnPropertyChanged(nameof(MonthlyIncomeBudget)); OnPropertyChanged(nameof(RemainingIncome)); }
        }


        public decimal RemainingExpense => MonthlyExpenseBudget - MonthlyExpense;

        public decimal RemainingIncome => MonthlyIncomeBudget - MonthlyIncome;
        




        /// <summary>
        /// Transaction user can edit and add to current month
        /// </summary>
        public TransactionModel NewTransaction { get; set; } = new TransactionModel();

        /// <summary>
        /// List of all transaction for current month
        /// </summary>
        public ObservableCollection<TransactionModel> Transactions { get; set; } = new ObservableCollection<TransactionModel>();

        public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
        public List<AccountModel> Accounts { get; set; } = new List<AccountModel>();

        #endregion Public Properties

        #region Commands
        /// <summary>
        /// Adds the <see cref="NewTransaction"/> to the <see cref="Transactions"/>
        /// </summary>
        public ICommand AddTransactionCommand { get; set; }

        /// <summary>
        /// Remove selected command
        /// </summary>
        public ICommand RemoveTransactionCommand { get; set; }

        #endregion Commands

        private readonly IDatabaseData _data;

        public TransactionsViewModel(IDatabaseData data)
        {
            //Assign data from dependency injection
            _data = data;

            //Load data
            foreach (CategoryModel c in data.GetCategories())
            {
                Categories.Add(c);
            }
            foreach (AccountModel a in data.GetAccounts())
            {
                Accounts.Add(a);
            }

            //Assign relay commands
            AddTransactionCommand = new RelayCommand<TransactionModel>(() => AddTransaction(), (p) => CanAddTransaction(p));
            RemoveTransactionCommand = new RelayCommand<TransactionModel>((p) => RemoveTransaction(p));

            //Setup user data
            CurrentMonth = DateTime.Now;
            ClearNewTransaction();
        }


        #region Add Transaction

        /// <summary>
        /// Determines whether the <see cref="NewTransaction"/> can be added to the month
        /// </summary>        
        bool CanAddTransaction(TransactionModel t)
        {
            return t != null && t.Amount > 0 && t.CategoryId > 0 && t.AccountId > 0;
        }

        /// <summary>
        /// Adds the <see cref="NewTransaction"/> to the transactions for current month
        /// </summary>
        void AddTransaction()
        {
            _data.CreateTransaction(NewTransaction);

            //Capture relevant data
            AccountModel account = Accounts.Single(x => x.Id == NewTransaction.AccountId);
            CategoryModel category = Categories.Single(x => x.Id == NewTransaction.CategoryId);

            //Add to display if valid
            if (NewTransaction.Date.Month == CurrentMonth.Month && NewTransaction.Date.Year == CurrentMonth.Year)
            {              

                Transactions.Add(new TransactionModel()
                {
                    Note = NewTransaction.Note,
                    Amount = NewTransaction.Amount,
                    Date = NewTransaction.Date,
                    IsIncome = category.IsIncome,
                    CategoryId = NewTransaction.CategoryId,
                    AccountId = NewTransaction.AccountId,
                    Category = category.Name,
                    Account = account.Name
                });                

                if (category.IsIncome)
                {
                    MonthlyIncome += NewTransaction.Amount;
                }
                else
                {
                    MonthlyExpense += NewTransaction.Amount;                   
                }               
            }

            //Update the account amount
            if(category.IsIncome)
            {
                account.Amount += NewTransaction.Amount;
            }
            else
            {
                account.Amount -= NewTransaction.Amount;
            }

            _data.UpdateAccountAmount(account.Id, account.Amount);


            //Reset user interface
            ClearNewTransaction();

        }

        #endregion Add Transaction

        /// <summary>
        /// Loads transactions for <see cref="CurrentMonth"/>
        /// </summary>
        void LoadTransactions()
        {
            //Clear displayed transactions
            Transactions.Clear();
            MonthlyIncome = 0M;
            MonthlyExpense = 0M;
            MonthlyIncomeBudget = 0M;
            MonthlyExpenseBudget = 0M;

            //Get transactions for the selected date
            List<TransactionModel> transactions = _data.GetTransactions(CurrentMonth.Year, CurrentMonth.Month);
            List<BudgetAmountModel> budget = _data.GetBudgetAmounts(CurrentMonth.Year, CurrentMonth.Month);

            //Test if no transactions exist for month
            if (transactions == null)
            {
                return;
            }

            //Display transactions
            foreach (TransactionModel t in transactions)
            {
                Transactions.Add(t);

                if (t.IsIncome)
                {
                    MonthlyIncome += t.Amount;
                }
                else
                {
                    MonthlyExpense += t.Amount;
                }
            }

            //Display the budget remaining
            foreach(BudgetAmountModel b in budget)
            {
                if(b.IsIncome)
                {
                    MonthlyIncomeBudget += b.Amount;
                }
                else
                {
                    MonthlyExpenseBudget += b.Amount;
                }
            }

        }

        public void RemoveTransaction(TransactionModel transaction)
        {
            if (transaction == null) return;
            
            Transactions.Remove(transaction);
            _data.RemoveTransaction(transaction.Id);

            AccountModel account = Accounts.Single(x => x.Id == transaction.AccountId);

            if (transaction.IsIncome)
            {
                MonthlyIncome -= transaction.Amount;
                account.Amount -= transaction.Amount;

            }
            else
            {
                MonthlyExpense -= transaction.Amount;
                account.Amount += transaction.Amount;
            }

            _data.UpdateAccountAmount(account.Id, account.Amount);

        }

        /// <summary>
        /// Resets the <see cref="NewTransaction"/> for user
        /// </summary>
        void ClearNewTransaction()
        {
            NewTransaction.Note = "";
            NewTransaction.Amount = 0;
            NewTransaction.Date = new DateTime(CurrentMonth.Year, CurrentMonth.Month, CurrentMonth.Day);
            NewTransaction.IsIncome = false;

        }
    }
}
