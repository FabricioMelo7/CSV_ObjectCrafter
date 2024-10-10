using CSV_ObjectCrafter.Enums;
using CSV_ObjectCrafter.Utils;
using Microsoft.Win32;
using System.Configuration;
using System.Windows.Input;

namespace CSV_ObjectCrafter.ViewModels
{
    public class CommandButtonsViewModel
    {
        public bool FileLoaded { get; set; }
        public ICommand ImportCommand { get; }
        public ICommand ExportCommand { get; }

        public List<Themes> AvailableThemes { get; set; }

        public delegate void ImportEventHandler(object sender, HelperEventArgs e);
        public event ImportEventHandler ImportButtonPressed;

        public delegate void ExportEventHandler(object sender, HelperEventArgs e);
        public event ExportEventHandler ExportButtonPressed;

        public delegate void ThemeChangedEventHandler(object sender, HelperEventArgs e);
        public event ThemeChangedEventHandler ThemeChangedEvent;

        public CommandButtonsViewModel()
        {
            ImportCommand = new RelayCommand<object>(Import, CanImport);
            ExportCommand = new RelayCommand<object>(Export, CanExport);
            AvailableThemes = new List<Themes>() { Themes.Dark, Themes.Light };
        }

        private bool CanImport(object? obj)
        {
            return true;
        }

        private void Import(object? obj)
        {
            ImportButtonPressed(this, new HelperEventArgs { FilePath = OpenFileDialog() });
        }

        private bool CanExport(object? obj)
        {
            return FileLoaded;
        }

        private void Export(object? obj)
        {
            ExportButtonPressed?.Invoke(this, new HelperEventArgs { FilePath = SaveFileDialog() });
        }

        private string SaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Export",
                Filter = "All Files (*.*)|*.*",
                DefaultExt = "csv",
                FileName = "Any"

            };

            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : string.Empty;
        }

        private string OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Import",
                Filter = "All Files (*.*)|*.*",
                DefaultExt = "csv",
                FileName = "Any"
            };

            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : string.Empty;
        }

        public void ThemeChanged(string theme)
        {
            ThemeChangedEvent?.Invoke(this, new HelperEventArgs { ThemeColor = GetThemeColor(theme) });
        }

        private Themes GetThemeColor(string obj)
        {
            return obj == "Dark" ? Themes.Dark : Themes.Light;
        }
    }
}
