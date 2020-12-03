using GoDollar.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GoDollar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Tabs.SelectedIndex = 0;
        }
     
        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Tabs.SelectedIndex)
            {
                case 0:
                    Content.Content = new TransactionsControl();
                    break;
                case 1:
                    Content.Content = new BudgetControl();
                    break;
                case 2:
                    Content.Content = new UserDataControl();
                    break;

                default:
                    break;
            }
        }
    }
}
