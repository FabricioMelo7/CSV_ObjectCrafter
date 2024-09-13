using CSV_ObjectCrafter.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSV_ObjectCrafter.ViewModels
{
    public class CommandButtonsViewModel
    {
        public ICommand ImportCommand { get; }
        public ICommand ExportCommand { get; }

        public CommandButtonsViewModel()
        {
            ImportCommand = new RelayCommand<object>(Import, CanImport);
            ExportCommand = new RelayCommand<object>(Export, CanExport);
        }

        private bool CanImport(object? obj)
        {
            return true;
        }

        private void Import(object? obj)
        {
            OpenFileDialog();
        }

        private bool CanExport(object? obj)
        {
            return true;
        }

        private void Export(object? obj)
        {
            SaveFileDialog();
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
    }
}
