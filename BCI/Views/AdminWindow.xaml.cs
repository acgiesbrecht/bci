using BCI.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Linq;
using System.Reflection;
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

namespace BCI.Views
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : MetroWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        AdminViewModel viewModel;

        public bool isLoading { get; set; }
        public AdminWindow()
        {

            isLoading = true;
            InitializeComponent();

            viewModel = new AdminViewModel();
            DataContext = viewModel;
            viewModel.NotificationEvent += NotificationEvent;

            try
            {
                //reset();

                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    this.Title = "BCI - " + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                }
                else
                {
                    this.Title = "BCI - " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
                }
            }
            catch (Exception ex)
            {
                viewModel.showError(ex);
            }
            isLoading = false;
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

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ErrorLinkVisibility = Visibility.Hidden;
            viewModel.StatusColor = this.WindowTitleBrush;
            ErrorWindow errorWindow = new ErrorWindow();
            errorWindow.ActualException = viewModel.ActualException;
            errorWindow.ShowDialog();
        }

        private void InventoryItemsAutoCompleteComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            viewModel.SelectedInventoryItemChanged();
        }

        private void TipoActividadAutoCompleteComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            viewModel.SelectedTipoActividadChanged();
        }

        private void OrganisationAutoCompleteComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                viewModel.SelectedOrganisationChanged();
            }
            catch (Exception ex)
            {
                viewModel.showError(ex);
            }
        }

        private void EstablecimientoAutoCompleteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.SelectedEstabChanged();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //viewModel.loadData();
                reset();
                viewModel.resetEditFields();
            }
            catch (Exception ex)
            {
                viewModel.showError(ex);
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!viewModel.validate())
                {
                    return;
                }
                MessageBoxResult result = MessageBox.Show("Está seguro que desea guardar el registro actual?",
                                              "Confirmation",
                                              MessageBoxButton.YesNo,
                                              MessageBoxImage.Question);
                //MessageBoxResult result = await this.ShowMessage("This is the title", "Some message");
                if (result == MessageBoxResult.Yes)
                {
                    viewModel.Save();
                    reset();
                }
            }
            catch (Exception ex)
            {
                viewModel.showError(ex);
            }
        }

        private void reset()
        {
            try
            {
                viewModel.resetEditFields();

                InventoryItemsAutoCompleteComboBox.IsEnabled = false;
                ItemsUpdateBtn.IsEnabled = false;
                TipoActividadAutoCompleteComboBox.IsEnabled = false;
                OrganisationAutoCompleteComboBox.IsEnabled = false;
                PuntosOperacionAutoCompleteComboBox.IsEnabled = false;
                MatriculaTextBox.IsEnabled = false;
                EstablecimientoAutoCompleteComboBox.IsEnabled = false;
                ContratoAutoCompleteComboBox.IsEnabled = false;
                LoteAutoCompleteComboBox.IsEnabled = false;
                RemisionTextBox.IsEnabled = false;
                ObservacionesTextBox.IsEnabled = false;
            }
            catch (Exception ex)
            {
                viewModel.showError(ex);
            }
        }

        private void ContratoAutoCompleteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.SelectedContratoChanged();
        }

        private void PesadasCerradasDataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (!isLoading)
                {
                    reset();
                    PuntosOperacionAutoCompleteComboBox.IsEnabled = true;
                    MatriculaTextBox.IsEnabled = true;
                    EstablecimientoAutoCompleteComboBox.IsEnabled = true;
                    ContratoAutoCompleteComboBox.IsEnabled = true;
                    //LoteAutoCompleteComboBox.IsEnabled = true;
                    RemisionTextBox.IsEnabled = true;
                    ObservacionesTextBox.IsEnabled = true;
                    viewModel.SelectedPesadaChanged();
                }
            }
            catch (Exception ex)
            {
                viewModel.showError(ex);
            }
        }

        private void EstabUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateEstablecimientoPanel();
        }

        private void ItemsUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            //viewModel.UpdateInventoryItemsPanel();
            viewModel.loadLOV();
        }

        private void BtnUpdateCerradasGrid_Click(object sender, RoutedEventArgs e)
        {
            viewModel.loadData();
            reset();
            viewModel.resetEditFields();
        }
    }
}
