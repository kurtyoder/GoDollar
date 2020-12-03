using GoDollar.ViewModels;
using GoDollarLibrary.Data;
using GoDollarLibrary.Databases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GoDollar
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Set up dependency injection
            var services = new ServiceCollection();
            services.AddTransient<MainWindow>();
            services.AddTransient<TransactionsViewModel>();
            services.AddTransient<BudgetViewModel>();
            services.AddTransient<UserDataViewModel>();
            services.AddTransient<ISqliteDataAccess, SqliteDataAccess>();

            //Allow for a configuration file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = builder.Build();

            services.AddSingleton(config);

            //Determine which database to use
            string dbChoice = config.GetValue<string>("DatabaseChoice").ToLower();
            if(dbChoice == "sqlite")
            {
                services.AddTransient<IDatabaseData, SqliteData>();
            }
            else
            {
                throw new NotImplementedException();
            }


            serviceProvider = services.BuildServiceProvider();

            //Get our main window from dependency injection
            MainWindow mainWindow = serviceProvider.GetService<MainWindow>();

            //Start the main window
            mainWindow.Show();
        }
    }
}
