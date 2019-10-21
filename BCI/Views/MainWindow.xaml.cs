using BCI.Models;
using BCI.ViewModels;
using DotNetKit.Windows.Controls;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Deployment.Internal;
using System.Deployment.Application;
using System.Windows.Media;

namespace BCI
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        MainViewModel viewModel;
        public bool isLoading { get; set; }
        public MainWindow()
        {
            isLoading = true;            

            InitializeComponent();

            viewModel = new MainViewModel();
            DataContext = viewModel;
            viewModel.NotificationEvent += NotificationEvent;

            try
            {
                reset();

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
            viewModel.AutoBascula = true;
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

        private void BtnBruto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                viewModel.PesoBruto = viewModel.PesoActual;
            }
            catch (Exception ex)
            {
                viewModel.showError(ex);
            }
        }

        private void BtnTara_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                viewModel.PesoTara = viewModel.PesoActual;
            }
            catch (Exception ex)
            {
                viewModel.showError(ex);
            }
        }

        private void BtnNewPesada_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                viewModel.CreateNewPesada();

                InventoryItemsAutoCompleteComboBox.IsEnabled = true;
                InventoryItemsAutoCompleteComboBox.IsEnabled = true;
                TipoActividadAutoCompleteComboBox.IsEnabled = true;
                OrganisationAutoCompleteComboBox.IsEnabled = true;
                PuntosOperacionAutoCompleteComboBox.IsEnabled = true;
                MatriculaTextBox.IsEnabled = true;
                EstablecimientoAutoCompleteComboBox.IsEnabled = true;
                ContratoAutoCompleteComboBox.IsEnabled = true;
                LoteAutoCompleteComboBox.IsEnabled = true;
                RemisionTextBox.IsEnabled = true;
                ObservacionesTextBox.IsEnabled = true;
            }
            catch (Exception ex)
            {
                viewModel.showError(ex);
            }
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

        /*private void PesadasPendientesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isLoading)
            {
                reset();
                ObservacionesTextBox.IsEnabled = true;
                viewModel.SelectedPesadaPendientesChanged();                
            }
        }*/

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

        private void NewLoteBtn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.CreateNewLoteAlgodon();
        }

        private void ClearBrutoBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                viewModel.PesoBruto = 0;
            }
            catch (Exception ex)
            {
                viewModel.showError(ex);
            }
        }
        private void ClearTaraBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                viewModel.PesoTara = 0;
            }
            catch (Exception ex)
            {
                viewModel.showError(ex);
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ErrorLinkVisibility = Visibility.Hidden;
            viewModel.StatusColor = this.WindowTitleBrush;
            ErrorWindow errorWindow = new ErrorWindow();
            errorWindow.ActualException = viewModel.ActualException;
            errorWindow.ShowDialog();
        }

        private void ContratoAutoCompleteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.SelectedContratoChanged();
        }

        private void PesadasPendientesDataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
                    LoteAutoCompleteComboBox.IsEnabled = true;
                    RemisionTextBox.IsEnabled = true;
                    ObservacionesTextBox.IsEnabled = true;
                    viewModel.SelectedPesadaPendientesChanged();
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

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        private void CerradasGridTicketMenuItem_Click(object sender, RoutedEventArgs e)
        {
            viewModel.imprimirTicket((XX_OPM_BCI_PESADAS_ALL)PesadasCompletasDataGrid.SelectedItem);
        }

        private void CerradasGridCertificadoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            viewModel.imprimirCertificado((XX_OPM_BCI_PESADAS_ALL)PesadasCompletasDataGrid.SelectedItem);
        }

        private void PendientesGridAutorizacionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            viewModel.imprimirAutorizacion((XX_OPM_BCI_PESADAS_ALL)PesadasPendientesDataGrid.SelectedItem);
        }

        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            viewModel.serialStop();
        }

        private void PendientesGridMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PendientesGridTicketMuestraMenuItem_Click(object sender, RoutedEventArgs e)
        {
            viewModel.imprimirTicketMuestra((XX_OPM_BCI_PESADAS_ALL)PesadasPendientesDataGrid.SelectedItem);
        }

        private void PendientesGridCancelarMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Está seguro que desea cancelar el registro actual?",
                                             "Confirmation",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);            
            if (result == MessageBoxResult.Yes)
            {
                viewModel.cancelarPesada((XX_OPM_BCI_PESADAS_ALL)PesadasPendientesDataGrid.SelectedItem);                
            }
        }

        private void CerradasGridCancelarMenuItem_Click(object sender, RoutedEventArgs e)
        {            
            MessageBoxResult result = MessageBox.Show("Está seguro que desea cancelar el registro actual?",
                                             "Confirmation",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                viewModel.cancelarPesada((XX_OPM_BCI_PESADAS_ALL)PesadasCompletasDataGrid.SelectedItem);
            }
        }
    }
}
