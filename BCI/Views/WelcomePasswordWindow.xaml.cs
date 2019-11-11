using BCI.ViewModels;
using BCI.Views;
using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace BCI
    {
    /// <summary>
    /// Interaction logic for WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomePasswordWindow : MetroWindow, INotifyPropertyChanged
        {
        public event PropertyChangedEventHandler PropertyChanged;

        WelcomeViewModel viewModel = new WelcomeViewModel();

        public WelcomePasswordWindow()
            {
            InitializeComponent();
            txtAnswer.Focus();
            }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (txtAnswer.Password.Equals("indu"))
            {
                this.DialogResult = true;
            }
            else
            {
                NotificationEvent(this, "Contraseña invalida!", true);
                txtAnswer.Focus();
            }
            
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }

        public string Answer
        {
            get { return txtAnswer.Password; }
        }

        private void NotificationEvent(object sender, string msg, bool isError)
        {
            NotificationWindow notificationWindow = new NotificationWindow();
            if (this.IsActive)
            {
                notificationWindow.Owner = this;
            }
            notificationWindow.Message = msg;
            notificationWindow.ShowTitleBar = false;
            if (isError)
            {
                notificationWindow.BorderBrush = new SolidColorBrush(Colors.Red);
                notificationWindow.Background = new SolidColorBrush(Colors.Pink);
            }
            else
            {
                notificationWindow.BorderBrush = new SolidColorBrush(Colors.Yellow);
                notificationWindow.Background = new SolidColorBrush(Colors.LightYellow);
            }
            notificationWindow.ShowDialog();
        }
    }
    }
