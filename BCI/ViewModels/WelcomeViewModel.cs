using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BCI.ViewModels
    {
    class WelcomeViewModel : INotifyPropertyChanged
        {
        public event PropertyChangedEventHandler PropertyChanged;

        public WelcomeViewModel() {}
    }
}
