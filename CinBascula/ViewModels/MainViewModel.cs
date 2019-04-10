using CinBascula.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CinBascula.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public XX_OPM_BCI_PESADAS_ALL PesadaActual;
        public bool NewPesada { get; set; }
        public bool UpdatePesada { get; set; }
        public bool NewOrUpdatePesada { get; set; }
        public int? PesoActual { get; set; }
        public int? PesoBruto { get; set; }
        public int? PesoTara { get; set; }

        private SerialPort serialPort = new SerialPort();
        public OracleDataManager oracleDataManager = new OracleDataManager();
        public XX_OPM_BCI_ITEMS_V SelectedInventoryItem { get; set; }
        public ObservableCollection<XX_OPM_BCI_ITEMS_V> InventoryItemsCollection { get; set; }
        public XX_OPM_BCI_TIPO_ACTIVIDAD SelectedTipoActividad { get; set; }
        public ObservableCollection<XX_OPM_BCI_TIPO_ACTIVIDAD> TiposActividadCollection { get; set; }
        public XX_OPM_BCI_ORGS_COMPLEJO SelectedOrganisation { get; set; }
        public ObservableCollection<XX_OPM_BCI_ORGS_COMPLEJO> OrganisationsCollection { get; set; }        
        public string SelectedMatricula { get; set; }
        public Visibility PuntoOperacionVisibility { get; set; }
        public string PuntoOperacionLabel { get; set; }
        public XX_OPM_BCI_PUNTO_OPERACION SelectedPuntoOperacion { get; set; }
        public ObservableCollection<XX_OPM_BCI_PUNTO_OPERACION> PuntosOperacionCollection { get; set; }
        public ObservableCollection<XX_OPM_BCI_PUNTO_OPERACION> PuntosDescargaCollection { get; set; }
        public ObservableCollection<XX_OPM_BCI_PUNTO_OPERACION> PuntosCargaCollection { get; set; }
        public ObservableCollection<XX_OPM_BCI_ESTAB> EstabsAPCollection { get; set; }
        public ObservableCollection<XX_OPM_BCI_ESTAB> EstabsARCollection { get; set; }
        public ObservableCollection<XX_OPM_BCI_ESTAB> EstabServiciosCollection { get; set; }
        public string EstablecimientoLabel { get; set; }
        public XX_OPM_BCI_ESTAB SelectedEstab { get; set; }
        public ObservableCollection<XX_OPM_BCI_ESTAB> EstabsCollection { get; set; }
        public Visibility LoteVisibility { get; set; }
        public XX_OPM_BCI_LOTE SelectedLote { get; set; }
        public ObservableCollection<XX_OPM_BCI_LOTE> LotesCollection { get; set; }
        public Visibility ContratoVisibility {get; set;}
        public XX_OPM_BCI_CONTRATOS_V SelectedContrato { get; set; }
        public ObservableCollection<XX_OPM_BCI_CONTRATOS_V> ContratosCollection { get; set; }        
        public ObservableCollection<XX_OPM_BCI_PESADAS_ALL> PesadasAllCollection { get; set; }
        public XX_OPM_BCI_PESADAS_ALL SelectedPesadaPendiente { get; set; }
        public ICollectionView PesadasPendientesView { get; set; }
        public ICollectionView PesadasCompletasView { get; set; }
        private bool autoBascula;
        public bool AutoBascula
        {
            get => autoBascula;
            set
            {
                autoBascula = value;
                if (autoBascula) { serialStart(); } else { serialStop(); }
            }
        }
        public bool ManualBascula { get { return !AutoBascula; } }
        public bool BtnBrutoIsEnabled { get; set; }
        public bool BtnTaraIsEnabled { get; set; }
        public bool BtnGuardarIsEnabled { get; set; }

        public MainViewModel()
        {
            loadData();                        
        }

        public void CreateNewPesada()
        {
            PesadaActual = new XX_OPM_BCI_PESADAS_ALL();
            NewPesada = true;
            BtnBrutoIsEnabled = true;
            BtnTaraIsEnabled = true;
            BtnGuardarIsEnabled = true;
        }

        public void loadData()
        {
            InventoryItemsCollection = new ObservableCollection<XX_OPM_BCI_ITEMS_V>(oracleDataManager.GetInventoryItemList());
            TiposActividadCollection = new ObservableCollection<XX_OPM_BCI_TIPO_ACTIVIDAD>(oracleDataManager.GetTipoActividadList());
            OrganisationsCollection = new ObservableCollection<XX_OPM_BCI_ORGS_COMPLEJO>(oracleDataManager.GetOrgsComplejoList());
            PuntosDescargaCollection = new ObservableCollection<XX_OPM_BCI_PUNTO_OPERACION>(oracleDataManager.GetPuntoDescargaList());
            PuntosCargaCollection = new ObservableCollection<XX_OPM_BCI_PUNTO_OPERACION>(oracleDataManager.GetPuntoCargaList());
            EstabsAPCollection = new ObservableCollection<XX_OPM_BCI_ESTAB>(oracleDataManager.GetEstabAPList());
            EstabsARCollection = new ObservableCollection<XX_OPM_BCI_ESTAB>(oracleDataManager.GetEstabARList());
            EstabServiciosCollection = new ObservableCollection<XX_OPM_BCI_ESTAB>();
            XX_OPM_BCI_ESTAB estab = new XX_OPM_BCI_ESTAB();
            estab.Id = "9999";
            estab.RazonSocial = "Cliente Servicios";
            EstabServiciosCollection.ToList().Add(estab);

            ContratosCollection = new ObservableCollection<XX_OPM_BCI_CONTRATOS_V>(oracleDataManager.GetContratosList());            

            PesadasAllCollection = new ObservableCollection<XX_OPM_BCI_PESADAS_ALL>(oracleDataManager.GetPesadas());
            PesadasAllCollection.ToList().ForEach(x => x.InventoryItem = InventoryItemsCollection.FirstOrDefault(c => c.INVENTORY_ITEM_ID.Equals(x.INVENTORY_ITEM_ID)));
            PesadasAllCollection.ToList().ForEach(x => x.TipoActividad = TiposActividadCollection.FirstOrDefault(c => c.Id.Equals(x.TIPO_ACTIVIDAD)));
            PesadasAllCollection.ToList().ForEach(x => x.Organisation = OrganisationsCollection.FirstOrDefault(c => c.Id.Equals(x.ORGANIZATION_ID)));            
            
            foreach (XX_OPM_BCI_PESADAS_ALL p in PesadasAllCollection)
            {
                if (p.TIPO_ACTIVIDAD == 2)
                {
                    p.Establecimiento = EstabsARCollection.FirstOrDefault(c => c.Id.Equals(p.ESTABLECIMIENTO));
                    p.PuntoOperacion = PuntosCargaCollection.FirstOrDefault(c => c.Id.Equals(p.PUNTO_DESCARGA));
                }
                else
                {
                    p.Establecimiento = EstabsAPCollection.FirstOrDefault(c => c.Id.Equals(p.ESTABLECIMIENTO));
                    p.PuntoOperacion = PuntosDescargaCollection.FirstOrDefault(c => c.Id.Equals(p.PUNTO_DESCARGA));
                }
            }
            PesadasAllCollection.ToList().ForEach(x => x.Contrato = ContratosCollection.FirstOrDefault(c => c.NRO_CONTRATO.Equals(x.CONTRATO)));
            PesadasPendientesView = new CollectionViewSource { Source = PesadasAllCollection }.View;            
            PesadasPendientesView.Filter = o =>
            {
                XX_OPM_BCI_PESADAS_ALL p = o as XX_OPM_BCI_PESADAS_ALL;
                return p.PESO_BRUTO == null
                       || p.PESO_TARA == null;
            };
            PesadasPendientesView.SortDescriptions.Add(
                new SortDescription("EntryDate", ListSortDirection.Descending));
            PesadasPendientesView.Refresh();

            PesadasCompletasView = new CollectionViewSource { Source = PesadasAllCollection }.View;            
            PesadasCompletasView.Filter = o =>
            {
                XX_OPM_BCI_PESADAS_ALL p = o as XX_OPM_BCI_PESADAS_ALL;
                return p.PESO_BRUTO != null
                       && p.PESO_TARA != null;
            };
            PesadasCompletasView.SortDescriptions.Add(
                new SortDescription("ExitDate", ListSortDirection.Descending));
            PesadasCompletasView.Refresh();
        }

        public void SelectedInventoryItemChanged()
        {
            if (SelectedInventoryItem != null)
            {
                SelectedTipoActividad = TiposActividadCollection.FirstOrDefault(t => t.Id == SelectedInventoryItem.TIPO_ACTIVIDAD);
                SelectedOrganisation = OrganisationsCollection.FirstOrDefault(o => o.Id == SelectedInventoryItem.ORGANIZATION_ID);                
            }
            if (SelectedEstab != null && SelectedInventoryItem != null)
            {
                UpdateLotePanel();
            }
        }

        public void SelectedTipoActividadChanged()
        {
            if (SelectedTipoActividad != null)
            {                
                switch (SelectedTipoActividad.Id)
                {
                    case 1L:
                    EstablecimientoLabel = "Proveedor";
                    PuntoOperacionLabel = "Punto de Descarga";
                    EstabsCollection = EstabsAPCollection;
                        break;
                    case 2L:
                    EstablecimientoLabel = "Cliente";
                        EstabsCollection = EstabsARCollection;
                    PuntoOperacionLabel = "Punto de Carga";
                    break;
                    case 3L:
                        EstablecimientoLabel = "";                        
                        EstabsCollection = EstabServiciosCollection;
                        PuntoOperacionLabel = "";
                        break;
                    default:
                    EstablecimientoLabel = "Establecimiento";
                    PuntoOperacionLabel = "Punto de Descarga";
                    EstabsCollection = EstabsAPCollection;                        
                    break;
                }
            }            
        }

            public void SelectedOrganisationChanged()
        {            
            if (SelectedOrganisation != null)
            {
                PuntosOperacionCollection = new ObservableCollection<XX_OPM_BCI_PUNTO_OPERACION>(PuntosDescargaCollection.Where(o => o.Tag.Equals(SelectedOrganisation.Tag)));                
            }
        }

        public void SelectedEstabChanged()
        {
            if (SelectedEstab != null && SelectedInventoryItem != null)
            {
                UpdateLotePanel();
            }
        }

        public void SelectedPesadaPendientesChanged()
        {
            if (SelectedPesadaPendiente != null)
            {
                reset();
                PesadaActual = SelectedPesadaPendiente;
                SelectedInventoryItem = InventoryItemsCollection.FirstOrDefault(i => i.INVENTORY_ITEM_ID.Equals(PesadaActual.INVENTORY_ITEM_ID));
                SelectedTipoActividad = TiposActividadCollection.FirstOrDefault(i => i.Id.Equals(PesadaActual.TIPO_ACTIVIDAD));
                SelectedOrganisation = OrganisationsCollection.FirstOrDefault(i => i.Id.Equals(PesadaActual.ORGANIZATION_ID));
                
                SelectedMatricula = PesadaActual.MATRICULA;
                if (SelectedTipoActividad.Id == 2){
                    SelectedPuntoOperacion = PuntosOperacionCollection.FirstOrDefault(i => i.Id.Equals(PesadaActual.PUNTO_DESCARGA));
                    SelectedEstab = EstabsARCollection.FirstOrDefault(i => i.Id.Equals(PesadaActual.ESTABLECIMIENTO));
                }else{
                    SelectedPuntoOperacion = PuntosOperacionCollection.FirstOrDefault(i => i.Id.Equals(PesadaActual.PUNTO_DESCARGA));
                    SelectedEstab = EstabsAPCollection.FirstOrDefault(i => i.Id.Equals(PesadaActual.ESTABLECIMIENTO));
                }
                if (PesadaActual.CONTRATO != null)
                {
                    SelectedContrato = ContratosCollection.FirstOrDefault(i => i.NRO_CONTRATO.Equals(PesadaActual.CONTRATO));
                }
                if (PesadaActual.LOTE != null)
                {
                    UpdateLotePanel();                    
                }
                if (PesadaActual.PESO_BRUTO != null)
                {
                    PesoBruto = PesadaActual.PESO_BRUTO;
                    BtnBrutoIsEnabled = false;
                    BtnTaraIsEnabled = true;
                }
                if (PesadaActual.PESO_TARA != null)
                {
                    PesoTara = PesadaActual.PESO_TARA;
                    BtnBrutoIsEnabled = true;
                    BtnTaraIsEnabled = false;
                }
                BtnGuardarIsEnabled = true;
                UpdatePesada = true;
            }
        }

        private void UpdateContratoPanel()
        {
            if (SelectedInventoryItem != null && SelectedEstab != null)
            {
                ContratoVisibility = Visibility.Visible;
                ContratosCollection = new ObservableCollection<XX_OPM_BCI_CONTRATOS_V>(oracleDataManager.GetContratoByEstablecimientoAndItem(SelectedEstab, SelectedInventoryItem));
                    SelectedContrato = ContratosCollection.FirstOrDefault(i => i.NRO_CONTRATO.Equals(PesadaActual.CONTRATO));                
            }
            else
            {
                ContratoVisibility = Visibility.Hidden;
            }
        }

        private void UpdateLotePanel()
        {
            if (SelectedInventoryItem != null && SelectedEstab != null){ 
                if (SelectedInventoryItem.CODIGO_ITEM.Equals("050.002198"))
                {
                    LoteVisibility = Visibility.Visible;
                    LotesCollection = new ObservableCollection<XX_OPM_BCI_LOTE>(oracleDataManager.GetLotesAlgodonByEstablecimiento(SelectedEstab.Id));
                    SelectedLote = LotesCollection.FirstOrDefault(i => i.ID.Equals(PesadaActual.LOTE));
                }
                else
                {
                    LoteVisibility = Visibility.Hidden;
                }
            }
            else{
                LoteVisibility = Visibility.Hidden;
            }
        }

        public void CreateNewLote() {
            XX_OPM_BCI_LOTE maxLote = oracleDataManager.GetMaxLoteCurrentYear();            
            XX_OPM_BCI_LOTE newLote = new XX_OPM_BCI_LOTE();
            newLote.ID = maxLote.Year + "-" +
                (int.Parse(maxLote.LoteCodigo) + 1).ToString("D3") + "-" +
                SelectedEstab.Id;
            LotesCollection.Add(newLote);
            SelectedLote = newLote;
        }

        public void Save(){
            if (PesadaActual!=null)
            {
                if (NewPesada)
                {
                    PesadaActual.INVENTORY_ITEM_ID = SelectedInventoryItem.INVENTORY_ITEM_ID;
                    PesadaActual.TIPO_ACTIVIDAD = SelectedTipoActividad.Id;
                    PesadaActual.ORGANIZATION_ID = SelectedOrganisation.Id;
                    PesadaActual.PUNTO_DESCARGA = SelectedPuntoOperacion.Id.ToString();
                    PesadaActual.MATRICULA = SelectedMatricula;
                    PesadaActual.ESTABLECIMIENTO = SelectedEstab.Id;
                    if (SelectedContrato != null)
                    {
                        PesadaActual.CONTRATO = SelectedContrato.NRO_CONTRATO;
                    }
                    if (SelectedLote != null)
                    {
                        PesadaActual.LOTE = SelectedLote.ID;
                    }
                    if (PesoBruto != null)
                    {
                        PesadaActual.PESO_BRUTO = PesoBruto;
                        PesadaActual.FECHA_PESO_BRUTO = DateTime.Now;
                        PesadaActual.MODO_PESO_BRUTO = AutoBascula == true ? 'A' : 'M';
                    }
                    if (PesoTara != null)
                    {
                        PesadaActual.PESO_TARA = PesoTara;
                        PesadaActual.FECHA_PESO_TARA = DateTime.Now;
                        PesadaActual.MODO_PESO_TARA = AutoBascula == true ? 'A' : 'M';
                    }
                    oracleDataManager.insertNewPesada(PesadaActual);
                }
                else if(UpdatePesada){
                    if (PesadaActual.PESO_BRUTO == null && PesoBruto != null)
                    {
                        PesadaActual.PESO_BRUTO = PesoBruto;
                        PesadaActual.FECHA_PESO_BRUTO = DateTime.Now;
                        PesadaActual.MODO_PESO_BRUTO = AutoBascula == true ? 'A' : 'M';
                    }
                    if (PesadaActual.PESO_TARA == null && PesoTara != null)
                    {
                        PesadaActual.PESO_TARA = PesoTara;
                        PesadaActual.FECHA_PESO_TARA = DateTime.Now;
                        PesadaActual.MODO_PESO_TARA = AutoBascula == true ? 'A' : 'M';
                    }
                    PesadaActual.LAST_UPDATE_DATE = DateTime.Now;
                    oracleDataManager.updatePesada(PesadaActual);
                }
                reset();
                loadData();
            }          
            }

        public void reset()
        {
            UpdatePesada = false;
            NewPesada = false;
            PesoActual = null;
            PesoBruto = null;
            PesoTara = null;
            PesadaActual = null;
            
            SelectedInventoryItem = null;
            SelectedTipoActividad = null;
            SelectedOrganisation = null;
            SelectedPuntoOperacion = null;
            SelectedMatricula = null;
            SelectedEstab = null;
            SelectedContrato = null;
            SelectedLote = null;
            //UpdateLotePanel();            

            AutoBascula = true;

            BtnBrutoIsEnabled = false;
            BtnTaraIsEnabled = false;
            BtnGuardarIsEnabled = false;            
        }

        private void serialStart()
        {
            serialPort.PortName = "COM1";
            serialPort.BaudRate = 9600;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.ReadTimeout = 400;
            serialPort.ReadBufferSize = 64;
            serialPort.Open();
            serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(SerialPortRecieve);
        }

        private void serialStop()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        private void SerialPortRecieve(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                PesoActual = int.Parse(serialPort.ReadLine().Substring(0, 4).Trim());
            }
            catch (Exception ex)
            {
                PesoActual = -1;
            }

        }

    }
}
