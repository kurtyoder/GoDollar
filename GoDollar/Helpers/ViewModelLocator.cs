using GoDollar.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoDollar.Helpers
{
    public class ViewModelLocator
    {
        public TransactionsViewModel TransactionViewModel => App.serviceProvider.GetRequiredService<TransactionsViewModel>();
        public BudgetViewModel BudgetViewModel => App.serviceProvider.GetRequiredService<BudgetViewModel>();
        public UserDataViewModel UserDataViewModel => App.serviceProvider.GetRequiredService<UserDataViewModel>();
    }
}
