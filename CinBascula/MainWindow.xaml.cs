using CinBascula.Models;
using CinBascula.ViewModels;
using DotNetKit.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CinBascula
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        MainViewModel viewModel = new MainViewModel();                        

        
                        
        public MainWindow()
        {

            DataContext = viewModel;
            InitializeComponent();

            reset();
            //viewModel.loadData();               
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
            
            BtnBruto.IsEnabled = true;
            BtnTara.IsEnabled = true;
            BtnGuardar.IsEnabled = true;

            InventoryItemsAutoCompleteComboBox.IsEnabled = true;
            TipoActividadAutoCompleteComboBox.IsEnabled = true;
            OrganisationAutoCompleteComboBox.IsEnabled = true;
            PuntosOperacionAutoCompleteComboBox.IsEnabled = true;
            MatriculaTextBox.IsEnabled = true;
            EstablecimientoAutoCompleteComboBox.IsEnabled = true;            
        }

        private void PesadasPendientesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.SelectedPesadaPendientesChanged();            
        }        

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Save();  
        }

        private void reset()
        {
            viewModel.reset();            

            InventoryItemsAutoCompleteComboBox.SelectedIndex = -1;
            InventoryItemsAutoCompleteComboBox.IsEnabled = false;
            TipoActividadAutoCompleteComboBox.SelectedIndex = -1;
            TipoActividadAutoCompleteComboBox.IsEnabled = false;
            OrganisationAutoCompleteComboBox.SelectedIndex = -1;
            OrganisationAutoCompleteComboBox.IsEnabled = false;
            PuntosOperacionAutoCompleteComboBox.SelectedIndex = -1;
            PuntosOperacionAutoCompleteComboBox.IsEnabled = false;
            MatriculaTextBox.Text = null;
            MatriculaTextBox.IsEnabled = false;
            EstablecimientoAutoCompleteComboBox.SelectedIndex = -1;
            EstablecimientoAutoCompleteComboBox.IsEnabled = false;
            ContratoAutoCompleteComboBox.SelectedIndex = -1;
            ContratoAutoCompleteComboBox.IsEnabled = false;
            LoteAutoCompleteComboBox.SelectedIndex = -1;
            LoteAutoCompleteComboBox.IsEnabled = false;
            PesoTextBox.Text = null;            
            BtnBruto.IsEnabled = false;
            BtnTara.IsEnabled = false;
            BtnGuardar.IsEnabled = false;
        }

        private void NewLoteBtn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.CreateNewLote();
        }
    }
}
