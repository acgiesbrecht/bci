﻿using BCI.Models;
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
        MainViewModel viewModel = new MainViewModel();
        public bool isLoading { get; set; }
        public MainWindow()
        {
            isLoading = true;
            DataContext = viewModel;
            InitializeComponent();
            try { 
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
            try { 
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
            try { 
                viewModel.PesoBruto = viewModel.PesoActual;
            }
            catch (Exception ex)
            {
                viewModel.showError(ex);
            }
        }

        private void BtnTara_Click(object sender, RoutedEventArgs e)
        {
            try { 
                viewModel.PesoTara = viewModel.PesoActual;
            }
            catch (Exception ex)
            {
                viewModel.showError(ex);
            }
        }
        
        private void BtnNewPesada_Click(object sender, RoutedEventArgs e)
        {
            try { 
                viewModel.CreateNewPesada();
            
                InventoryItemsAutoCompleteComboBox.IsEnabled = true;
                TipoActividadAutoCompleteComboBox.IsEnabled = true;
                OrganisationAutoCompleteComboBox.IsEnabled = true;
                PuntosOperacionAutoCompleteComboBox.IsEnabled = true;
                MatriculaTextBox.IsEnabled = true;
                EstablecimientoAutoCompleteComboBox.IsEnabled = true;           
                ContratoAutoCompleteComboBox.IsEnabled = true;
                LoteAutoCompleteComboBox.IsEnabled = true;
                ObservacionesTextBox.IsEnabled = true;
            } catch (Exception ex)
            {
                viewModel.showError(ex);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            try { 
                viewModel.loadData();
                reset();
                viewModel.resetEditFields();
            }catch (Exception ex)
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
            try { 
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
                }
            }
            catch (Exception ex)
            {
                viewModel.showError(ex);
            }
        }

        private void reset()
        {
            try { 
                viewModel.resetEditFields();            
            
                InventoryItemsAutoCompleteComboBox.IsEnabled = false;            
                TipoActividadAutoCompleteComboBox.IsEnabled = false;            
                OrganisationAutoCompleteComboBox.IsEnabled = false;            
                PuntosOperacionAutoCompleteComboBox.IsEnabled = false;            
                MatriculaTextBox.IsEnabled = false;            
                EstablecimientoAutoCompleteComboBox.IsEnabled = false;            
                ContratoAutoCompleteComboBox.IsEnabled = false;            
                LoteAutoCompleteComboBox.IsEnabled = false;    
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
            try { 
            viewModel.PesoBruto = 0;
            }
            catch (Exception ex)
            {
                viewModel.showError(ex);
            }
        }
        private void ClearTaraBtn_Click(object sender, RoutedEventArgs e)
        {
            try { 
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
            try { 
            if (!isLoading)
            {
                reset();
                PuntosOperacionAutoCompleteComboBox.IsEnabled = true;
                MatriculaTextBox.IsEnabled = true;
                EstablecimientoAutoCompleteComboBox.IsEnabled = true;
                ContratoAutoCompleteComboBox.IsEnabled = true;
                LoteAutoCompleteComboBox.IsEnabled = true;
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
            viewModel.UpdateInventoryItemsPanel();
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
            {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
            }
        }
}