using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GoDollar.UserControls
{
    /// <summary>
    /// Interaction logic for MonthPicker.xaml
    /// </summary>
    public partial class MonthPicker : UserControl
    {


        public DateTime Month
        {
            get => (DateTime)GetValue(MonthProperty);
            set => SetValue(MonthProperty, value);
        }

        // Using a DependencyProperty as the backing store for Month.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MonthProperty = DependencyProperty.Register("Month", typeof(DateTime), typeof(MonthPicker), new PropertyMetadata(DateTime.Now));       
              

        public MonthPicker()
        {
            InitializeComponent();           
        }    

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Month = Month.AddMonths(-1);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Month = Month.AddMonths(1);
        }
    }
}
