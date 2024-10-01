using CSV_ObjectCrafter.ViewModels;
using System.Windows.Controls;

namespace CSV_ObjectCrafter.Views
{
    public partial class CommandButtons : UserControl
    {
        public CommandButtons()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = DataContext as CommandButtonsViewModel;
            var t = sender as ComboBox;
            vm.ThemeChanged(t.SelectedItem.ToString());
        }
    }
}
