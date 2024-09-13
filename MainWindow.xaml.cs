using CSV_ObjectCrafter.ViewModels;
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
        public MainViewModel? MainViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            MainViewModel = DataContext as MainViewModel;
        }

        private void myDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}