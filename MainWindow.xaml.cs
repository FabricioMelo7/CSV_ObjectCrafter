using CSV_ObjectCrafter.Enums;
using CSV_ObjectCrafter.Utils;
using CSV_ObjectCrafter.ViewModels;
using System.Data;
using System.Dynamic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CSV_ObjectCrafter
{
    public partial class MainWindow : Window
    {
        MainViewModel viewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            viewModel = DataContext as MainViewModel;

            viewModel.UpdateDataGridEvent += UpdateDataGrid;
            viewModel.ChangeThemeColorEvent += UpdateThemeColor;
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
            
            viewModel.DataGridSelectionChange(rowView);
        }

        private void myDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "AbsoluteID")
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

        private void myDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            viewModel.CellModify(sender, e);
        }

        private void UpdateThemeColor(object sender, HelperEventArgs e)
        {
            UpdateDataGridTheme(e.ThemeColor);
            UpdateWindowTheme(e.ThemeColor);
            myCommandButtons.ThemeLabel.Foreground = e.ThemeColor.Equals(Themes.Dark) ? Brushes.WhiteSmoke : Brushes.Black;            
        }

        private void UpdateDataGridTheme(Themes themes)
        {
            myDataGrid.Background = themes.Equals(Themes.Dark) ? Brushes.Black : Brushes.White;
            myDataGrid.Foreground = themes.Equals(Themes.Dark) ? Brushes.Gray : Brushes.Black;
            myDataGrid.RowBackground = themes.Equals(Themes.Dark) ? Brushes.Black : Brushes.WhiteSmoke;
        }

        private void UpdateWindowTheme(Themes themes)
        {
            myWindow.Background = themes.Equals(Themes.Dark) ? Brushes.Black : Brushes.WhiteSmoke;
        }        
    }
}