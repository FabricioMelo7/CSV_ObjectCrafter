using CSV_ObjectCrafter.Enums;
using System.Data;

namespace CSV_ObjectCrafter.Utils
{
    public class HelperEventArgs
    {
        public string? FilePath { get; set; }
        public DataTable? dataTable { get; set; }
        public Themes ThemeColor { get; set; }

    }
}
