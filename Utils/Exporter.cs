using Microsoft.Win32;
using System.Dynamic;
using System.IO;
using System.Text;

namespace CSV_ObjectCrafter.Utils
{
    public static class Exporter
    {
        public static void ExportDataToCsv(string? filePath, List<ExpandoObject>? records)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                WriteToFile(filePath, SerializeCollection(records));
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

        private static string SerializeCollection(List<ExpandoObject> records)
        {
            var safeRecords = new List<ExpandoObject>();

            foreach (var record in records)
            {
                var newRecord = new ExpandoObject();
                var newRecordDict = (IDictionary<string, object>)newRecord;

                foreach (var kvp in (IDictionary<string, object>)record)
                {
                    if (kvp.Key != "AbsoluteID")
                    {
                        newRecordDict[kvp.Key] = kvp.Value;
                    }
                }

                safeRecords.Add(newRecord);
            }

            var csvBuilder = new StringBuilder();

            var firstRecordDic = (IDictionary<string, object>)safeRecords[0];
            var headers = string.Join(",", firstRecordDic.Keys);
            csvBuilder.AppendLine(headers);

            foreach (var record in safeRecords)
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
