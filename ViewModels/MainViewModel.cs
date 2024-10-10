using CSV_ObjectCrafter.Enums;
using CSV_ObjectCrafter.Utils;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Windows;
using System.Windows.Controls;

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
        public object? SelectedObject
        {
            get => _SelectedObject;
            set
            {
                _SelectedObject = value;
                OnPropertyChanged(nameof(SelectedObject));
            }
        }

        public delegate void UpdateDataGridHandler(object sender, HelperEventArgs e);
        public event UpdateDataGridHandler? UpdateDataGridEvent;

        public delegate void ChangeThemeColorHandler(object sender, HelperEventArgs e);
        public event ChangeThemeColorHandler? ChangeThemeColorEvent;

        private Themes _Theme;
        public Themes Theme
        {
            get => _Theme;
            set
            {
                _Theme = value;
                OnPropertyChanged(nameof(Theme));
            }
        }

        public MainViewModel()
        {
            commandButtonsVM = new CommandButtonsViewModel();
            commandButtonsVM.ImportButtonPressed += ParseImportedFile;
            commandButtonsVM.ExportButtonPressed += ExportCsvFile;
            commandButtonsVM.ThemeChangedEvent += ChangeThemeColor;

            dataTable = new DataTable();
        }

        private void ParseImportedFile(object sender, HelperEventArgs e)
        {
            if (Records == null || (Records != null && UserCheck()))
            {
                Records?.Clear();

                Records = (List<ExpandoObject>)Parser.ParseCsv(e.FilePath);
                SetDataGridColumns();
                SetDataGridRows();

                UpdateDataGridEvent?.Invoke(this, new HelperEventArgs { dataTable = dataTable });
            }

            commandButtonsVM.FileLoaded = true;
        }

        private void ExportCsvFile(object sender, HelperEventArgs e)
        {
            Exporter.ExportDataToCsv(e.FilePath, Records);
        }

        private void SetDataGridColumns()
        {
            SetHeaders();

            if (dataTable.Columns.Count > 0) { dataTable.Columns.Clear(); }

            foreach (string hd in Headers.Distinct())
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

            Headers.Add("AbsoluteID"); // Although this won't be showing in the DataGrid, it has to be added as an invisible collumn so that the program can iterate through the objects ID's. Makes life easier
        }

        private void SetDataGridRows()
        {
            dataTable.Rows.Clear();

            foreach (var record in Records)
            {
                var row = dataTable.NewRow();

                var recordDict = (IDictionary<string, object>)record;

                foreach (DataColumn column in dataTable.Columns)
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

        public void DataGridSelectionChange(DataRowView rowView)
        {
            if (rowView != null)
            {
                string? id = rowView["AbsoluteID"]?.ToString();

                if (!string.IsNullOrEmpty(id))
                {
                    var selectedItem = Records?.FirstOrDefault(r =>
                    {
                        var recordDic = (IDictionary<string, object>)r;

                        if (recordDic.ContainsKey("AbsoluteID"))
                        {
                            return recordDic["AbsoluteID"]?.ToString() == id;
                        }

                        return false;
                    });

                    SelectedObject = selectedItem ?? null;
                }
            }
        }

        public void CellModify(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditingElement is TextBox textBox)
            {
                var editedColumn = e.Column.Header.ToString();
                var newValue = textBox.Text;

                if (SelectedObject is IDictionary<string, object> recordDict && recordDict.ContainsKey(editedColumn) && newValue != string.Empty)
                {
                    recordDict[editedColumn] = newValue;
                    recordDict["DefaultEntry"] = false;
                }
            }            

            InsertNewBlankObject();
        }

        private void InsertNewBlankObject()
        {
            if (!EmptyItemExist(Records)) // If last row is not 'Default empty object' - Inserts a new empty row object
            {
                Records.Add(Parser.CreateBlankRowObject(Headers));
                SetDataGridRows();
            }
        }

        // Makes sure the last row is a 'Default empty object'
        private bool EmptyItemExist(List<ExpandoObject> _records)
        {
            foreach (ExpandoObject record in _records)
            {
                if (record is IDictionary<string, object> recordDict && recordDict.ContainsKey("DefaultEntry") && recordDict["DefaultEntry"] is true)
                {
                    return true;
                }
            }

            return false;
        }


        private bool UserCheck()
        {
            MessageBoxResult result = MessageBox.Show(
            "Importing a new CSV file will delete the current one, any changes made will be lost",
            "Do you want to contine ?",
            MessageBoxButton.OKCancel,
            MessageBoxImage.Warning);

            return result == MessageBoxResult.OK ? true : false;
        }

        private void ChangeThemeColor(object sender, HelperEventArgs e)
        {
            Theme = e.ThemeColor;
            ChangeThemeColorEvent?.Invoke(this, new HelperEventArgs { ThemeColor = Theme });
        }
    }
}