using BCI.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BCI
    {
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : MetroWindow, INotifyPropertyChanged
        {
        public event PropertyChangedEventHandler PropertyChanged;

        SettingsViewModel viewModel = new SettingsViewModel();

        public SettingsWindow()
            {
            DataContext = viewModel;
            InitializeComponent();
            SerialPortAutoCompleteComboBox.SelectedItem = Properties.Settings.Default.SerialPort;
            TicketPrinterAutoCompleteComboBox.SelectedItem = Properties.Settings.Default.TicketPrinter;
            }

        private void Guardar_Click(object sender, RoutedEventArgs e)
            {
            Properties.Settings.Default.SerialPort = SerialPortAutoCompleteComboBox.SelectedItem.ToString();
            Properties.Settings.Default.TicketPrinter = TicketPrinterAutoCompleteComboBox.SelectedItem.ToString();
            Properties.Settings.Default.Save();
            this.Close();
            }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
            {
                this.Close();
            }
        }
    }
