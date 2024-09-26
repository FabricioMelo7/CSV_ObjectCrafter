using Microsoft.Win32;
using System.Dynamic;
using System.IO;
using System.Text;

namespace CSV_ObjectCrafter.Utils
{
    public static class Exporter
    {
        public static void ExportDataToCsv(string? filePath, List<ExpandoObject>? records, List<string>? headers)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                WriteToFile(filePath, SerializeCollection(records, headers));
            }
        }

        private static string? ShowSaveFileDialog(string title, string fileName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = title,
                Filter = "CSV files (*.csv)|*.csv",
                DefaultExt = "csv",
                FileName = fileName
            };

            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
        }

        private static void WriteToFile(string filePath, string? content)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.Write(content);
            }
        }

        private static string SerializeCollection(List<ExpandoObject> records, List<string> headers)
        {
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendJoin(",", headers);

            foreach (var record in records)
            {
                var recordDict = (IDictionary<string, object>)record;

                var values = new List<string>();

                foreach (var key in recordDict.Keys)
                {
                    var value = recordDict[key]?.ToString() ?? string.Empty;
                    values.Add(value);
                }

                var line = string.Join(",", values);
                csvBuilder.AppendLine(line);
            }

            return csvBuilder.ToString();
        }
    }
}
