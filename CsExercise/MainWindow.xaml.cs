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

namespace CsExercise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var currencyList = CurrencyFileMgr.GetCurrenciesWeb(int.Parse(currenciesComboBox.SelectedValue.ToString()),
                startDate.SelectedDate.Value, endDate.SelectedDate.Value);
            //var currencyList = CurrencyFileMgr.GetCurrenciesCsv(csvPathTextBox.Text);

            GraphWindow graphWindow = new GraphWindow(currencyList);
            graphWindow.ShowDialog();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            currenciesComboBox.ItemsSource = CurrencyFileMgr.GetCurrencies();
            currenciesComboBox.DisplayMemberPath = "Value";
            currenciesComboBox.SelectedValuePath = "Key";
            currenciesComboBox.SelectedIndex = 1; 
        }
    }
}
