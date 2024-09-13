using CSV_ObjectCrafter.Utils;
using CSV_ObjectCrafter.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ObservableCollection<string> Headers { get; set; }
        public ObservableCollection<object> Objects { get; set; }

        

        public MainViewModel()
        {
            commandButtonsVM = new CommandButtonsViewModel();
            
        }

        
    }
}
