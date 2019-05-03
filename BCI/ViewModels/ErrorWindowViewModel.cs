using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.ViewModels
{
    class ErrorWindowViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public Exception ActualException { get; set; }
        public String Message {
            get
            {
                return ActualException.Message + "\n" + ActualException.StackTrace.ToString();
            }
            set
            {
                return;
            }
        }

        public String InnerExMessage
        {
            get
            {
                if (ActualException.InnerException != null)
                {
                    return ActualException.InnerException.Message + "\n" + ActualException.InnerException.StackTrace.ToString();
                }
                else
                {
                    return null;
                }
                
            }
            set
            {
                return;
            }
        }

    }
}
