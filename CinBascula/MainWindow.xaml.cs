﻿using CinBascula.Models;
using CinBascula.ViewModels;
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

namespace CinBascula
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        MainViewModel viewModel = new MainViewModel();                        
        
        public MainWindow()
        {
            DataContext = viewModel;
            InitializeComponent();

            reset();
            //viewModel.loadData();               
            //this.Title = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                this.Title = "BCI - " + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            else
            {
                this.Title = "BCI - " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
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
            viewModel.SelectedOrganisationChanged();
            PuntosOperacionAutoCompleteComboBox.SelectedIndex = 0;
        }

        private void EstablecimientoAutoCompleteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.SelectedEstabChanged();
        }

        private void BtnBruto_Click(object sender, RoutedEventArgs e)
        {
            viewModel.PesoBruto = viewModel.PesoActual;
        }

        private void BtnTara_Click(object sender, RoutedEventArgs e)
        {
            viewModel.PesoTara = viewModel.PesoActual;
        }
        
        private void BtnNewPesada_Click(object sender, RoutedEventArgs e)
        {
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
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {            
            viewModel.loadData();
            reset();
            viewModel.resetEditFields();
        }

        private void PesadasPendientesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            reset();
            viewModel.SelectedPesadaPendientesChanged();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!viewModel.validate())
            {
                return;
            }
            MessageBoxResult result = MessageBox.Show("Está seguro que desea guardar el registro actual?",
                                          "Confirmation",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);           
            if (result == MessageBoxResult.Yes)
            {
                viewModel.Save();
            }
        }

        private void reset()
        {
            viewModel.resetEditFields();            
            
            InventoryItemsAutoCompleteComboBox.IsEnabled = false;            
            TipoActividadAutoCompleteComboBox.IsEnabled = false;            
            OrganisationAutoCompleteComboBox.IsEnabled = false;            
            PuntosOperacionAutoCompleteComboBox.IsEnabled = false;            
            MatriculaTextBox.IsEnabled = false;            
            EstablecimientoAutoCompleteComboBox.IsEnabled = false;            
            ContratoAutoCompleteComboBox.IsEnabled = false;            
            LoteAutoCompleteComboBox.IsEnabled = false;                   
        }        

        private void NewLoteBtn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.CreateNewLoteAlgodon();
        }

        private void ButtonStatus_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(viewModel.StatusMessage);
        }

        private void ClearBrutoBtn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.PesoBruto = 0;
        }
        private void ClearTaraBtn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.PesoTara = 0;
        }
    }
}
