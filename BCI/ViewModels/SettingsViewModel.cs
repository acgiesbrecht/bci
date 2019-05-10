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
    class SettingsViewModel : INotifyPropertyChanged
        {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> PortList = new ObservableCollection<string>();
        public ObservableCollection<string> PrinterList = new ObservableCollection<string>();
        public ICollectionView PortView { get; set; }
        public ICollectionView PrinterView { get; set; }
        public SettingsViewModel() {

            foreach (string printer in PrinterSettings.InstalledPrinters)
                {
                PrinterList.Add(printer);
                }
            PrinterView = new CollectionViewSource { Source = PrinterList }.View;
            foreach (string port in SerialPort.GetPortNames())
                {
                PortList.Add(port);
                }
            PortView = new CollectionViewSource { Source = PortList }.View;
            }

        public void Save()
            {
            //Properties.Settings.Default.SerialPort = SelectedPort;
            Properties.Settings.Default.Save();
            }

        public string SelectedPort { set; get; }
        public string SelectedPrinter { set; get; }

        }
    }
