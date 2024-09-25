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
        MainViewModel viewModel {  get; set; }
        public MainWindow()
        {
            InitializeComponent();
            viewModel = DataContext as MainViewModel;          
            
            viewModel.UpdateDataGridEvent += UpdateDataGrid;
        }

        private void UpdateDataGrid(object sender, HelperEventArgs e)
        {
            DataView dv = new DataView(e.dataTable);
            myDataGrid.ItemsSource = dv;
        }

        private void myDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            DataRowView? rowView = dataGrid?.SelectedItem as DataRowView;

            if (rowView != null)
            {                
                string? id = rowView["AbsoluteID"]?.ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    var selectedItem = viewModel.Records?.FirstOrDefault(r =>
                    {
                        var recordDic = (IDictionary<string, object>)r;

                        if (recordDic.ContainsKey("AbsoluteID"))
                        {
                            return recordDic["AbsoluteID"]?.ToString() == id;
                        }

                        return false;
                    });

                    viewModel.SelectedObject = selectedItem ?? null;
                }
                else
                {
                    viewModel.SelectedObject = null;
                }
            }
        }

        private void myDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if(e.PropertyName == "AbsoluteID")
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }
    }
}