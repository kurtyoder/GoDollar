using GoDollarLibrary.Databases;
using GoDollarLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoDollarLibrary.Data
{
    public class SqliteData : IDatabaseData
    {
        private const string connectionStringName = "SqliteDb";
        private readonly ISqliteDataAccess _db;

        public SqliteData(ISqliteDataAccess db)
        {
            _db = db;
        }

        public List<TransactionModel> GetTransactions(int year, int month)
        {
            //Get the date range
            var startDate = new DateTime(year, month, 1);
            var endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            return GetTransactions(startDate, endDate);
        }
        public List<TransactionModel> GetTransactions(DateTime startDate, DateTime endDate)
        {
            string sql = @"SELECT t.*, c.Name AS Category, c.IsIncome, a.Name AS Account FROM Transactions t LEFT JOIN Accounts a ON a.Id = t.AccountId LEFT JOIN Categories c ON c.Id = t.CategoryId WHERE Date BETWEEN @startDate AND @endDate;";

            List<TransactionModel> output = _db.Query<TransactionModel, dynamic>(sql, new { startDate, endDate }, connectionStringName);

            output.ForEach(x => x.Amount /= 100);

            return output;
        }

        public List<TransactionAmountModel> GetTransactionsAmounts(int year, int month)
        {
            //Get the date range
            var startDate = new DateTime(year, month, 1);
            var endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            string sql = @"SELECT CategoryId, Amount FROM Transactions WHERE Date BETWEEN @startDate AND @endDate;";

            List<TransactionAmountModel> output = _db.Query<TransactionAmountModel, dynamic>(sql, new { startDate, endDate }, connectionStringName);

            output.ForEach(x => x.Amount /= 100);

            return output;
        }

        public void CreateTransaction(TransactionModel transaction)
        {
            string sql = @"INSERT INTO Transactions (IsIncome, CategoryId, AccountId, Amount, Note, Date) VALUES (@IsIncome, @CategoryId, @AccountId, @transactionAmount, @Note, @Date);";

            int transactionAmount = (int)(transaction.Amount * 100);          

            _db.Execute<dynamic>(sql, new { transaction.IsIncome, transaction.CategoryId, transaction.AccountId, transactionAmount, transaction.Note, transaction.Date }, connectionStringName);
        }

        public void RemoveTransaction(int id)
        {
            string sql = @"DELETE FROM Transactions WHERE Id=@id";

            _db.Execute<dynamic>(sql, new { id }, connectionStringName);
        }

      

        public List<CategoryModel> GetCategories()
        {
            string sql = @"SELECT * FROM Categories;";

            return _db.Query<CategoryModel, dynamic>(sql, new { }, connectionStringName);
        }

        public List<AccountModel> GetAccounts()
        {
            string sql = @"SELECT * FROM Accounts;";

            List<AccountModel> output =  _db.Query<AccountModel, dynamic>(sql, new { }, connectionStringName);

            output.ForEach(x => x.Amount /= 100);

            return output;
        }

        public List<BudgetItemModel> GetBudget(int year, int month)
        {
            //Get the date range
            var startDate = new DateTime(year, month, 1);
            var endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            return GetBudget(startDate, endDate);
        }
        public List<BudgetItemModel> GetBudget(DateTime startDate, DateTime endDate)
        {
            string sql = @"SELECT b.*, c.Name AS CategoryName, c.IsIncome FROM BudgetItems b LEFT JOIN Categories c ON c.Id = b.CategoryId WHERE Date BETWEEN @startDate AND @endDate;";

            List<BudgetItemModel> output = _db.Query<BudgetItemModel, dynamic>(sql, new { startDate, endDate }, connectionStringName);

            output.ForEach(x => x.Amount /= 100);

            return output;
        }

        public List<BudgetAmountModel> GetBudgetAmounts(int year, int month)
        {            
            var startDate = new DateTime(year, month, 1);
            var endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            string sql = @"SELECT b.Amount, c.IsIncome FROM BudgetItems b LEFT JOIN Categories c ON c.Id = b.CategoryId WHERE Date BETWEEN @startDate AND @endDate";

            List<BudgetAmountModel> output = _db.Query<BudgetAmountModel, dynamic>(sql, new { startDate, endDate }, connectionStringName);

            output.ForEach(x => x.Amount /= 100);

            return output;
        }

        public void CreateBudget(List<BudgetItemModel> budget)
        {
            var param = new List<dynamic>();

            foreach(BudgetItemModel b in budget)
            {
                int amount = 0;

                param.Add(new { b.Date, b.CategoryId, amount });
            }

            string sql = @"INSERT INTO BudgetItems (Date, CategoryId, Amount) VALUES (@Date, @CategoryId, @amount);";

            _db.Execute(sql, param, connectionStringName);
        }

        public void SaveBudget(List<BudgetItemModel> budget)
        {
            var param = new List<dynamic>();

            foreach(BudgetItemModel b in budget)
            {
                int amount = (int)(b.Amount * 100);

                param.Add(new { b.Id, amount });
            }

            string sql = @"UPDATE BudgetItems SET Amount = @amount WHERE Id=@Id;";

            _db.Execute(sql, param, connectionStringName);
        }
        
        public void RemoveBudget(int year, int month)
        {
            //Get the date range
            var startDate = new DateTime(year, month, 1);
            var endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            RemoveBudget(startDate, endDate);
        }
        public void RemoveBudget(DateTime startDate, DateTime endDate)
        {
            string sql = @"DELETE FROM BudgetItems WHERE Date BETWEEN @startDate AND @endDate;";

            _db.Execute(sql, new { startDate, endDate }, connectionStringName);
        }

        public void AddCategory(CategoryModel c)
        {
            string sql = @"INSERT INTO Categories (Name, CreatedDate, IsIncome) VALUES (@Name, @CreatedDate, @IsIncome);";

            _db.Execute(sql, new { c.Name, c.CreatedDate, c.IsIncome }, connectionStringName);
        }

        public void RemoveCategory(int id)
        {
            string sql = @"DELETE FROM Categories WHERE Id=@id;";

            _db.Execute(sql, new { id }, connectionStringName);
        }

        public void EditCategory(CategoryModel c)
        {
            string sql = @"UPDATE Categories SET Name=@Name, IsIncome=@IsIncome WHERE Id=@Id;";

            _db.Execute(sql, new { c.Name, c.IsIncome, c.Id }, connectionStringName);
        }

        public void AddAccount(AccountModel a)
        {
            string sql = @"INSERT INTO Accounts (Name, DateCreated, Amount) VALUES (@Name, @DateCreated, @amount);";

            int amount = (int)(a.Amount * 100);

            _db.Execute(sql, new { a.Name, a.DateCreated, amount }, connectionStringName);
        }

        public void RemoveAccount(int id)
        {
            string sql = @"DELETE FROM Accounts WHERE Id=@id;";

            _db.Execute(sql, new { id }, connectionStringName);
        }

        public void EditAccount(AccountModel a)
        {
            string sql = @"UPDATE Accounts SET Name=@Name, Amount=@amount WHERE Id=@Id;";

            int amount = (int)(a.Amount * 100);

            _db.Execute(sql, new { a.Name, amount, a.Id }, connectionStringName);
        }

        public void UpdateAccountAmount(int id, decimal amount)
        {
            string sql = @"UPDATE Accounts SET Amount = @wholeAmount WHERE Id=@id;";

            int wholeAmount = (int)(amount * 100);

            _db.Execute(sql, new { wholeAmount, id }, connectionStringName);
        }
    }
}
