using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_ObjectCrafter.Utils
{
    public static class Parser
    {
        public static IEnumerable<ExpandoObject> ParseCsv(string filePath)
        {
            var records = new List<ExpandoObject>();

            using (var reader = new StreamReader(filePath))
            {
                var headerLine = reader.ReadLine();

                if (headerLine is null) throw new Exception("CSV file is empty or has invalid format");

                var headers = headerLine.Split(',');

                string? line;

                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(",");

                    dynamic record = new ExpandoObject();
                    var recordDict = (IDictionary<string, object>)record;

                    for (int i = 0; i < headers.Length; i++)
                    {
                        string header = headers[i];
                        string? value = i < values.Length ? values[i] : null;
                        recordDict[header] = value;
                    }

                    records.Add(record);
                }
            }

            return records;
        }
    }
}
