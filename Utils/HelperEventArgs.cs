using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_ObjectCrafter.Utils
{
    public class HelperEventArgs
    {
        public string? FilePath { get; set; }
        public DataTable? dataTable { get; set; }
    }
}
