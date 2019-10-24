using BCI.ViewModels;
using BCI.Views;
using MahApps.Metro.Controls;
using System.ComponentModel;

namespace BCI
    {
    /// <summary>
    /// Interaction logic for WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : MetroWindow, INotifyPropertyChanged
        {
        public event PropertyChangedEventHandler PropertyChanged;

        WelcomeViewModel viewModel = new WelcomeViewModel();

        public WelcomeWindow()
            {
            InitializeComponent();
            }

        private void ButtonBascula_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ButtonAdmin_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
            this.Close();
        }
    }
    }
