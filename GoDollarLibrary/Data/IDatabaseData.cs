using GoDollarLibrary.Models;
using System;
using System.Collections.Generic;

namespace GoDollarLibrary.Data
{
    public interface IDatabaseData
    {
        void AddAccount(AccountModel a);
        void AddCategory(CategoryModel c);
        void CreateBudget(List<BudgetItemModel> budget);
        void CreateTransaction(TransactionModel transaction);
        void EditAccount(AccountModel a);
        void EditCategory(CategoryModel c);
        List<AccountModel> GetAccounts();
        List<BudgetItemModel> GetBudget(DateTime startDate, DateTime endDate);
        List<BudgetItemModel> GetBudget(int year, int month);
        List<BudgetAmountModel> GetBudgetAmounts(int year, int month);
        List<CategoryModel> GetCategories();
        List<TransactionModel> GetTransactions(DateTime startDate, DateTime endDate);
        List<TransactionModel> GetTransactions(int year, int month);
        List<TransactionAmountModel> GetTransactionsAmounts(int year, int month);
        void RemoveAccount(int id);
        void RemoveBudget(DateTime startDate, DateTime endDate);
        void RemoveBudget(int year, int month);
        void RemoveCategory(int id);
        void RemoveTransaction(int id);
        void SaveBudget(List<BudgetItemModel> budget);
        void UpdateAccountAmount(int id, decimal amount);
    }
}