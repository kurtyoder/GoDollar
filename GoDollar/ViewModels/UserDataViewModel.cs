using GoDollar.Helpers;
using GoDollarLibrary.Data;
using GoDollarLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace GoDollar.ViewModels
{
    public class UserDataViewModel : BaseViewModel
    {

        #region Public Properties
        private ObservableCollection<AccountModel> _accounts = new ObservableCollection<AccountModel>();

        public ObservableCollection<AccountModel> Accounts
        {
            get => _accounts;
            set 
            { 
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }

        private ObservableCollection<CategoryModel> _categories = new ObservableCollection<CategoryModel>();

        public ObservableCollection<CategoryModel> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        private CategoryModel _selectedCategory;

        public CategoryModel SelectedCategory
        {
            get => _selectedCategory;
            set 
            { 
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));                
            }
        }

        private AccountModel _selectedAccount;

        public AccountModel SelectedAccount
        {
            get => _selectedAccount;
            set 
            { 
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
            }
        }

        private bool _showAddCategoryPopup;

        public bool ShowAddCategoryPopup
        {
            get => _showAddCategoryPopup;
            set 
            { 
                _showAddCategoryPopup = value;
                OnPropertyChanged(nameof(ShowAddCategoryPopup));
            }
        }
        private bool _showEditCategoryPopup;

        public bool ShowEditCategoryPopup
        {
            get => _showEditCategoryPopup;
            set
            {
                _showEditCategoryPopup = value;
                OnPropertyChanged(nameof(ShowEditCategoryPopup));
            }
        }

        private bool _showAddAccountPopup;

        public bool ShowAddAccountPopup
        {
            get => _showAddAccountPopup;
            set
            {
                _showAddAccountPopup = value;
                OnPropertyChanged(nameof(ShowAddAccountPopup));
            }
        }

        private bool _showEditAccountPopup;

        public bool ShowEditAccountPopup
        {
            get => _showEditAccountPopup;
            set
            {
                _showEditAccountPopup = value;
                OnPropertyChanged(nameof(ShowEditAccountPopup));
            }
        }

        private AccountModel _newAccount = new AccountModel();

        public AccountModel NewAccount
        {
            get => _newAccount;
            set 
            { 
                _newAccount = value;
                OnPropertyChanged(nameof(NewAccount));
            }
        }

        private CategoryModel _newCategory = new CategoryModel();

        public CategoryModel NewCategory
        {
            get => _newCategory;
            set
            {
                _newCategory = value;
                OnPropertyChanged(nameof(NewCategory));
            }
        }



        #endregion Public Properties


        #region Private Fields

        /// <summary>
        /// Stores the original value of <see cref="SelectedAccount"/> in order to restore if necessary/>
        /// </summary>
        private AccountModel originalAccount = new AccountModel();

        /// <summary>
        /// Stores the original value of <see cref="SelectedAccount"/> in order to restore if necessary/>
        /// </summary>
        private CategoryModel originalCategory = new CategoryModel();

        #endregion Private  Fields


        #region Commands

        public ICommand AddAccountCommand { get; set; }
        public ICommand RemoveAccountCommand { get; set; }
        public ICommand SaveAccountCommand { get; set; }
        public ICommand AddCategoryCommand { get; set; }
        public ICommand RemoveCategoryCommand { get; set; }
        public ICommand SaveCategoryCommand { get; set; }
        public ICommand ToggleAddAccountPopupCommand { get; set; }
        public ICommand ToggleEditAccountPopupCommand { get; set; }
        public ICommand ToggleAddCategoryPopupCommand { get; set; }
        public ICommand ToggleEditCategoryPopupCommand { get; set; }


        #endregion Commands

        private readonly IDatabaseData _data;
        public UserDataViewModel(IDatabaseData data)
        {
            _data = data;

            //Assign relay commands
            AddAccountCommand = new RelayCommand<AccountModel>(() => AddAccount(), (p) => CanAddAccount(p));
            RemoveAccountCommand = new RelayCommand<AccountModel>(() => RemoveAccount(), (p) => HasAccount(p));
            SaveAccountCommand = new RelayCommand<AccountModel>(() => SaveAccount(), (p) => HasAccount(p));

            AddCategoryCommand = new RelayCommand<CategoryModel>(() => AddCategory(), (p) => CanAddCategory(p));
            RemoveCategoryCommand = new RelayCommand<CategoryModel>(() => RemoveCategory(), (p) => HasCategory(p));
            SaveCategoryCommand = new RelayCommand<CategoryModel>(() => SaveCategory(), (p) => HasCategory(p));

            ToggleAddAccountPopupCommand = new RelayCommand<object>(() => AddAccountPopup());
            ToggleEditAccountPopupCommand = new RelayCommand<object>(() => EditAccountPopup());

            ToggleAddCategoryPopupCommand = new RelayCommand<object>(() => AddCategoryPopup());
            ToggleEditCategoryPopupCommand = new RelayCommand<object>(() => EditCategoryPopup());


            //Load data
            foreach (CategoryModel c in _data.GetCategories())
            {
                Categories.Add(c);
            }

            foreach(AccountModel a in _data.GetAccounts())
            {
                Accounts.Add(a);
            }
        }

        #region Categories

        public void AddCategory()
        {
            NewCategory.CreatedDate = DateTime.Now;
            
            _data.AddCategory(NewCategory);
            
            Categories.Add(NewCategory);

            NewCategory = new CategoryModel();

            ShowAddCategoryPopup = false;
        }

        public void SaveCategory()
        {
            _data.EditCategory(SelectedCategory);

            ShowEditCategoryPopup = false;
        }

        public void RemoveCategory()
        {
            _data.RemoveCategory(SelectedCategory.Id);

            Categories.Remove(SelectedCategory);

            ShowEditCategoryPopup = false;
        }

        public bool CanAddCategory(CategoryModel c)
        {
            return c != null && !string.IsNullOrWhiteSpace(c.Name);
        }

        public bool HasCategory(CategoryModel c)
        {
            return c != null;
        }

        public void AddCategoryPopup()
        {
            ShowAddCategoryPopup = !ShowAddCategoryPopup;
        }

        public void EditCategoryPopup()
        {
            //Toggle the visibility
            ShowEditCategoryPopup = !ShowEditCategoryPopup;

            if(SelectedCategory != null)
            {
                if (ShowEditCategoryPopup)
                {
                    //Store the current values of the selected category in order to restore them
                    originalCategory.Name = SelectedCategory.Name;
                    originalCategory.IsIncome = SelectedCategory.IsIncome;
                }
                else
                {
                    //Restore original values as we are cancelling edit operation
                    SelectedCategory.Name = originalCategory.Name;
                    SelectedCategory.IsIncome = originalCategory.IsIncome;
                }

            }
            
        }

        #endregion Categories

        #region Accounts
        public void AddAccount()
        {
            NewAccount.DateCreated = DateTime.Now;

            _data.AddAccount(NewAccount);
            Accounts.Add(NewAccount);

            NewAccount = new AccountModel();

            ShowAddAccountPopup = false;

        }

        public void SaveAccount()
        {
            _data.EditAccount(SelectedAccount);

            ShowEditAccountPopup = false;
        }

        public void RemoveAccount()
        {
            _data.RemoveAccount(SelectedAccount.Id);

            Accounts.Remove(SelectedAccount);

            ShowEditAccountPopup = false;
        }


        public bool CanAddAccount(AccountModel a)
        {
            return a != null && a.Amount > 0 && !string.IsNullOrWhiteSpace(a.Name);
        }
        public bool HasAccount(AccountModel a)
        {
            return a != null;
        }

        public void AddAccountPopup()
        {
            //Toggle visibility of add account popup
            ShowAddAccountPopup = !ShowAddAccountPopup;
        }

        public void EditAccountPopup()
        {
            //Toggle visibility
            ShowEditAccountPopup = !ShowEditAccountPopup;


            if(SelectedAccount != null)
            {
                if (ShowEditAccountPopup)
                {
                    //Store the default values of the account so we can restore values
                    originalAccount.Amount = SelectedAccount.Amount;
                    originalAccount.Name = SelectedAccount.Name;
                }
                else
                {
                    //Restore the original values to selected account since we are canceling edit operation
                    SelectedAccount.Amount = originalAccount.Amount;
                    SelectedAccount.Name = originalAccount.Name;
                }
            }
           
        }

        #endregion Accounts


    }
}
