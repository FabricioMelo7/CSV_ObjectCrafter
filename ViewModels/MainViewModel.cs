using CSV_ObjectCrafter.Utils;
using CSV_ObjectCrafter.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CSV_ObjectCrafter.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CommandButtonsViewModel commandButtonsVM { get; set; }

        public List<string>? Headers { get; set; }

        public List<ExpandoObject>? Records { get; set; }
                
        public DataTable dataTable { get; set; }       

        private object? _SelectedObject;
        public object SelectedObject // This will be moved to the DataModifying window later on, will leave here for now...
        {
            get => _SelectedObject;
            set
            {
                _SelectedObject = value;
                OnPropertyChanged(nameof(SelectedObject));
            }            
        }

        public delegate void UpdateDataGridHandler(object sender, HelperEventArgs e);
        public event UpdateDataGridHandler UpdateDataGridEvent;

        public MainViewModel()
        {
            commandButtonsVM = new CommandButtonsViewModel();
            commandButtonsVM.ImportButtonPressed += ParseImportedFile;
            dataTable = new DataTable();
        }

        private void ParseImportedFile(object sender, HelperEventArgs e)
        {
            Records = (List<ExpandoObject>)Parser.ParseCsv(e.FilePath);
            SetDataGridColumns();
            SetDataGridRows();

            UpdateDataGridEvent?.Invoke(this, new HelperEventArgs { dataTable = dataTable});
        }

        private void SetDataGridColumns()
        {
            SetHeaders();

            foreach(string hd in Headers.Distinct())
            {
                DataColumn column = new DataColumn(hd, typeof(string));
                dataTable.Columns.Add(column);
            }
        }

        private void SetHeaders()
        {
            if (Headers == null)
            {
                Headers = new List<string>(Parser.Headers);
            }
            else
            {
                Headers.Clear();
                foreach (var header in Parser.Headers)
                {
                    Headers.Add(header);
                }
            }

            Headers.Add("AbsoluteID");
        }

        private void SetDataGridRows()
        {
            dataTable.Rows.Clear();

            foreach(var record in Records)
            {
                var row = dataTable.NewRow();

                var recordDict = (IDictionary<string, object>)record;

                foreach(DataColumn column in dataTable.Columns)
                {
                    if (recordDict.ContainsKey(column.ColumnName))
                    {
                        var propertyValue = recordDict[column.ColumnName];

                        row[column.ColumnName] = propertyValue;
                    }
                    else
                    {
                        row[column.ColumnName] = DBNull.Value;
                    }

                }

                dataTable.Rows.Add(row);
            }
        }
    }
}
