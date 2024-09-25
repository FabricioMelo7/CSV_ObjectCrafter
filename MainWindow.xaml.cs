using CSV_ObjectCrafter.Utils;
using CSV_ObjectCrafter.ViewModels;
using System.Data;
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

namespace CSV_ObjectCrafter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = DataContext as MainViewModel;          
            
            vm.UpdateDataGridEvent += UpdateDataGrid;
        }

        private void UpdateDataGrid(object sender, HelperEventArgs e)
        {
            DataView dv = new DataView(e.dataTable);
            myDataGrid.ItemsSource = dv;
        }

        private void myDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}