using CinBascula.Models;
using CinBascula.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Windows;

namespace CinBascula
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort serialPort = new SerialPort();
        private OracleDataManager oracleDataManager = new OracleDataManager();
        public ObservableCollection<XX_OPM_BCI_ITEMS_V> InventoryItemsCollection;
        public ObservableCollection<XX_OPM_BCI_TIPO_ACTIVIDAD> TipoActividadCollection;                
        public ObservableCollection<XX_OPM_BCI_ORGS_COMPLEJO> OrganisationCollection;
        public ObservableCollection<XX_OPM_BCI_PUNTO_DESCARGA> PuntoDescargaCollection;        
        public ObservableCollection<XX_OPM_BCI_ESTAB> EstabAllCollection;
        public ObservableCollection<XX_OPM_BCI_ESTAB> EstabAPCollection;
        public ObservableCollection<XX_OPM_BCI_ESTAB> EstabARCollection;
        public ObservableCollection<XX_OPM_BCI_CONTRATOS_V> ContratosCollection;
        public ObservableCollection<XX_OPM_BCI_PESADAS_ALL> PesadasPendientesList;

        public MainWindow()
        {
            InitializeComponent();
            loadLookUps();
            InventoryItemsCollection = new ObservableCollection<XX_OPM_BCI_ITEMS_V>(oracleDataManager.GetInventoryItemList());
            InventoryItemsAutoCompleteComboBox.ItemsSource = InventoryItemsCollection;

            TipoActividadAutoCompleteComboBox.ItemsSource = TipoActividadCollection;
            OrganisationAutoCompleteComboBox.ItemsSource = OrganisationCollection;
        }

        
        private void InventoryItemsAutoCompleteComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TipoActividadAutoCompleteComboBox.SelectedValue = ((XX_OPM_BCI_ITEMS_V)(InventoryItemsAutoCompleteComboBox.SelectedItem)).TIPO_ACTIVIDAD;
            OrganisationAutoCompleteComboBox.SelectedValue = ((XX_OPM_BCI_ITEMS_V)(InventoryItemsAutoCompleteComboBox.SelectedItem)).ORGANIZATION_ID;
        }

        private void loadLookUps()
        {
            EstabAllCollection = oracleDataManager.GetEstabAllList();
            EstabAPCollection = new ObservableCollection<XX_OPM_BCI_ESTAB>(EstabAllCollection.Where(t => t.ApAr.Equals("AP")));
            EstabARCollection = new ObservableCollection<XX_OPM_BCI_ESTAB>(EstabAllCollection.Where(t => t.ApAr.Equals("AR")));
            OrganisationCollection = oracleDataManager.GetOrgsComplejoList();
            PuntoDescargaCollection = oracleDataManager.GetPuntoDescargaList();
            TipoActividadCollection = oracleDataManager.GetTipoActividadList();
        }

        private void OrganisationAutoCompleteComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {                                        
                    PuntoDeDescargaAutoCompleteComboBox.ItemsSource = PuntoDescargaCollection.Where(o => o.Tag.Equals(((XX_OPM_BCI_ORGS_COMPLEJO)OrganisationAutoCompleteComboBox.SelectedItem).Tag));
                    PuntoDeDescargaAutoCompleteComboBox.SelectedIndex = 0;                
            }

        private void TipoActividadAutoCompleteComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {            
                switch (TipoActividadAutoCompleteComboBox.SelectedValue)
                {
                case "1":
                        TipoEstablecimientoLabel.Content = "Proveedor";
                        break;
                    case "2":
                        TipoEstablecimientoLabel.Content = "Cliente";
                        break;
                    default:
                        TipoEstablecimientoLabel.Content = "Establecimiento";
                        break;                
            }
        }
    }
}
