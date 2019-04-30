using BCI.Models;
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
using System.Windows.Media;

namespace BCI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public XX_OPM_BCI_PESADAS_ALL PesadaActual;
        public bool NewPesada { get; set; }
        public bool UpdatePesada { get; set; }
        public bool NewOrUpdatePesada { get; set; }
        public bool isLoading { get; set; }

        public int? PesoActual { get; set; }
        public int? PesoBruto { get; set; }
        public int? PesoTara { get; set; }

        private SerialPort serialPort = new SerialPort();
        public OracleDataManager oracleDataManager = new OracleDataManager();        
        public XX_OPM_BCI_ITEMS_V SelectedInventoryItem { get; set; }
        public ObservableCollection<XX_OPM_BCI_ITEMS_V> InventoryItemsCollection { get; set; }
        public ICollectionView InventoryItemsView { get; set; }

        public XX_OPM_BCI_TIPO_ACTIVIDAD SelectedTipoActividad { get; set; }
        public ObservableCollection<XX_OPM_BCI_TIPO_ACTIVIDAD> TiposActividadCollection { get; set; }
        public ICollectionView TiposActividadView { get; set; }
        public Visibility OrganisationVisibility { get; set; }
        public XX_OPM_BCI_ORGS_COMPLEJO SelectedOrganisation { get; set; }
        public ObservableCollection<XX_OPM_BCI_ORGS_COMPLEJO> OrganisationsCollection { get; set; }
        public ICollectionView OrganisationsView { get; set; }
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

        public Visibility EstablecimientoVisibility { get; set; }
        public string EstablecimientoLabel { get; set; }
        public XX_OPM_BCI_ESTAB SelectedEstab { get; set; }
        public ObservableCollection<XX_OPM_BCI_ESTAB> EstabsCollection { get; set; }
        public Visibility LoteVisibility { get; set; }
        public Visibility NewLoteBtnVisibility { get; set; }
        public XX_OPM_BCI_LOTE SelectedLote { get; set; }
        public ObservableCollection<XX_OPM_BCI_LOTE> LotesCollection { get; set; }
        public Visibility ContratoVisibility {get; set;}
        public XX_OPM_BCI_CONTRATOS_V SelectedContrato { get; set; }
        public ObservableCollection<XX_OPM_BCI_CONTRATOS_V> ContratosCollection { get; set; }
        public Visibility NotaRemisionVisibility { get; set; }
        public String SelectedRemisionNro { get; set; }
        public int? SelectedRemisionPeso { get; set; }
        public string SelectedObervaciones { get; set; }
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

        public Visibility ErrorLinkVisibility { get; set; }        
        public Exception ActualException { get; set; }
        public Brush StatusColor { get; set; }

        public MainViewModel()
        {
            isLoading = true;
            resetEditFields();
            resetTables();
            loadData();
            isLoading = false;
        }        

        public void loadData()
        {
            try
            {
                UpdateInventoryItemsPanel();

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
                /*return p.PESO_BRUTO == null
                       || p.PESO_TARA == null;*/
                    return !p.ESTADO.Equals("Cerrado");
                };
                PesadasPendientesView.SortDescriptions.Add(
                    new SortDescription("EntryDate", ListSortDirection.Descending));
                PesadasPendientesView.Refresh();

                PesadasCompletasView = new CollectionViewSource { Source = PesadasAllCollection }.View;
                PesadasCompletasView.Filter = o =>
                {
                    XX_OPM_BCI_PESADAS_ALL p = o as XX_OPM_BCI_PESADAS_ALL;
                /*return p.PESO_BRUTO != null
                       && p.PESO_TARA != null;*/
                    return p.ESTADO.Equals("Cerrado");
                };
                PesadasCompletasView.SortDescriptions.Add(
                    new SortDescription("ExitDate", ListSortDirection.Descending));
                PesadasCompletasView.Refresh();                
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void UpdateInventoryItemsPanel()
        {
            InventoryItemsCollection = new ObservableCollection<XX_OPM_BCI_ITEMS_V>(oracleDataManager.GetInventoryItemList());
            InventoryItemsView = new CollectionViewSource { Source = InventoryItemsCollection.Where(p => p.ORGANIZATION_ID != 0 && p == InventoryItemsCollection.First(i => i.CODIGO_ITEM == p.CODIGO_ITEM)) }.View;            
        }

        public void SelectedInventoryItemChanged()
        {
            if (SelectedInventoryItem != null)
            {
                UpdateTiposActividadPanel();
                UpdateOrganisationPanel();
            }
            UpdateLotePanel();
            UpdateContratoPanel();
        }

        private void UpdateTiposActividadPanel()
        {
            TiposActividadView = new CollectionViewSource
            {
                Source = TiposActividadCollection
                    .Where(p => InventoryItemsCollection
                        .Where(i => i.INVENTORY_ITEM_ID == SelectedInventoryItem.INVENTORY_ITEM_ID)
                        .Any(a => a.TIPO_ACTIVIDAD == p.Id))
            }.View;
            SelectedTipoActividad = (XX_OPM_BCI_TIPO_ACTIVIDAD)TiposActividadView.CurrentItem;
        }

        public void SelectedTipoActividadChanged()
        {
            if (SelectedTipoActividad != null)
            {
                BtnTaraIsEnabled = true;
                BtnBrutoIsEnabled = true;
                switch (SelectedTipoActividad.Id)
                {
                    case 1L:
                        OrganisationVisibility = Visibility.Visible;
                        PuntoOperacionLabel = "Punto de Descarga";
                        PuntoOperacionVisibility = Visibility.Visible;
                        EstablecimientoLabel = "Proveedor";
                        EstablecimientoVisibility = Visibility.Visible;
                        EstabsCollection = EstabsAPCollection;
                        if (NewPesada)
                        {                            
                            BtnTaraIsEnabled = false;
                        }
                        break;
                    case 2L:
                        OrganisationVisibility = Visibility.Visible;
                        PuntoOperacionLabel = "Punto de Carga";
                        PuntoOperacionVisibility = Visibility.Visible;
                        EstablecimientoLabel = "Cliente";
                        EstablecimientoVisibility = Visibility.Visible;                                                
                        EstabsCollection = EstabsARCollection;
                        if (NewPesada)
                        {                            
                            BtnBrutoIsEnabled = false;
                        }
                        break;
                    case 3L:
                        OrganisationVisibility = Visibility.Hidden;
                        EstablecimientoVisibility = Visibility.Hidden;
                        EstabsCollection = EstabServiciosCollection;
                        PuntoOperacionVisibility = Visibility.Hidden;                        
                        break;
                    default:
                        OrganisationVisibility = Visibility.Visible;
                        PuntoOperacionLabel = "Punto de Descarga";
                        PuntoOperacionVisibility = Visibility.Visible;
                        EstablecimientoLabel = "Establecimiento";
                        EstablecimientoVisibility = Visibility.Visible;
                        EstabsCollection = EstabsAPCollection;
                        break;
                }                
                UpdateOrganisationPanel();
            }            
        }

        private void UpdateOrganisationPanel()
        {                        
            if (SelectedTipoActividad != null)
            {
                OrganisationsView = new CollectionViewSource
                {
                    Source = OrganisationsCollection
                    .Where(p => InventoryItemsCollection
                        .Where(i => i.INVENTORY_ITEM_ID == SelectedInventoryItem.INVENTORY_ITEM_ID)
                        .Any(a => a.ORGANIZATION_ID == p.Id)
                        )
                    .Where(p => InventoryItemsCollection
                        .Where(i => i.TIPO_ACTIVIDAD == SelectedTipoActividad.Id)
                        .Any(a => a.ORGANIZATION_ID == p.Id))
                }.View;
            }
            else
            {
                OrganisationsView = new CollectionViewSource
                {
                    Source = OrganisationsCollection
                    .Where(p => InventoryItemsCollection
                        .Where(i => i.INVENTORY_ITEM_ID == SelectedInventoryItem.INVENTORY_ITEM_ID)
                        .Any(a => a.ORGANIZATION_ID == p.Id)                        
                        )
                }.View;
            }
            SelectedOrganisation = (XX_OPM_BCI_ORGS_COMPLEJO)OrganisationsView.CurrentItem;
        }

        public void SelectedOrganisationChanged()
        {            
            if (SelectedOrganisation != null)
            {
                if (SelectedTipoActividad != null)
                {
                    switch (SelectedTipoActividad.Id)
                    {
                        case 1L:
                            PuntosOperacionCollection = new ObservableCollection<XX_OPM_BCI_PUNTO_OPERACION>(PuntosDescargaCollection.Where(o => o.Tag.Equals(SelectedOrganisation.Tag)));                            
                            break;
                        case 2L:
                            PuntosOperacionCollection = new ObservableCollection<XX_OPM_BCI_PUNTO_OPERACION>(PuntosCargaCollection.Where(o => o.Tag.Equals(SelectedOrganisation.Tag)));                            
                            break;
                        default:
                            PuntosOperacionCollection = new ObservableCollection<XX_OPM_BCI_PUNTO_OPERACION>(PuntosDescargaCollection.Where(o => o.Tag.Equals(SelectedOrganisation.Tag)));
                            break;
                    }
                    SelectedPuntoOperacion = PuntosOperacionCollection.FirstOrDefault();
                }                
            }
        }

        public void SelectedEstabChanged()
        {            
            UpdateLotePanel();
            UpdateContratoPanel();
        }

        public void CreateNewPesada()
        {
            UpdateInventoryItemsPanel();
            resetEditFields();
            //resetTables();            
            PesadaActual = new XX_OPM_BCI_PESADAS_ALL();
            NewPesada = true;
            BtnBrutoIsEnabled = true;
            BtnTaraIsEnabled = true;
            BtnGuardarIsEnabled = true;            
        }

        public void SelectedPesadaPendientesChanged()
        {
            if (!isLoading && SelectedPesadaPendiente != null)
            {
                resetEditFields();
                PesadaActual = oracleDataManager.GetPesadaByID(SelectedPesadaPendiente.PESADA_ID);
                if (!PesadaActual.ESTADO.Equals("Completo"))
                {
                    MessageBox.Show("Aun no hay datos de calidad registradas.");                    
                    return;
                }
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
                SelectedObervaciones = PesadaActual.OBSERVACIONES;
                PesoBruto = PesadaActual.PESO_BRUTO;
                if (PesadaActual.PESO_BRUTO != null)
                {                    
                    BtnBrutoIsEnabled = false;
                    BtnTaraIsEnabled = PesadaActual.ESTADO.Equals("Completo");                    
                }
                PesoTara = PesadaActual.PESO_TARA;
                if (PesadaActual.PESO_TARA != null)
                {                    
                    BtnTaraIsEnabled = false;
                    BtnBrutoIsEnabled = PesadaActual.ESTADO.Equals("Completo");                                        
                }
                BtnGuardarIsEnabled = true;
                UpdatePesada = true;
            }
        }

        private void UpdateContratoPanel()
        {
            ContratoVisibility = Visibility.Hidden;
            if (SelectedInventoryItem != null && SelectedEstab != null && SelectedTipoActividad.Id == 1)
            {                
                ContratosCollection = new ObservableCollection<XX_OPM_BCI_CONTRATOS_V>(oracleDataManager.GetContratoByEstablecimientoAndItem(SelectedEstab, SelectedInventoryItem));
                if (ContratosCollection.Count > 0)
                {
                    ContratoVisibility = Visibility.Visible;
                    if(PesadaActual.CONTRATO != null)
                    {
                        SelectedContrato = ContratosCollection.FirstOrDefault(i => i.NRO_CONTRATO.Equals(PesadaActual.CONTRATO));
                    }
                    else
                    {
                        SelectedContrato = ContratosCollection.ElementAtOrDefault(0);
                    }                    
                }                
            }
        }

        public void SelectedContratoChanged()
        {
            UpdateNotaRemisionPanel();
        }

        private void UpdateNotaRemisionPanel()
        {
            NotaRemisionVisibility = Visibility.Hidden;
            if(SelectedContrato != null && SelectedContrato.PESO_ORIGEN == "SI")
            {
                NotaRemisionVisibility = Visibility.Visible;
            }
        }

        private void UpdateLotePanel()
        {
            LoteVisibility = Visibility.Hidden;
            NewLoteBtnVisibility = Visibility.Hidden;
            LotesCollection = new ObservableCollection<XX_OPM_BCI_LOTE>();
            if (SelectedInventoryItem != null && SelectedEstab != null && SelectedTipoActividad.Id == 1)
            {                
                if (SelectedInventoryItem.CODIGO_ITEM.Equals("050.002198"))
                {
                    LoteVisibility = Visibility.Visible;
                    if (NewPesada)
                    {
                        NewLoteBtnVisibility = Visibility.Visible;
                    }                    
                    LotesCollection = new ObservableCollection<XX_OPM_BCI_LOTE>(oracleDataManager.GetLotesAlgodonByEstablecimiento(SelectedEstab.Id));
                    SelectedLote = LotesCollection.FirstOrDefault(i => i.ID.Equals(PesadaActual.LOTE));
                }
                else if (SelectedInventoryItem.CODIGO_ITEM.Equals("050.001895"))
                {
                    LoteVisibility = Visibility.Visible;
                    LotesCollection = new ObservableCollection<XX_OPM_BCI_LOTE>(oracleDataManager.GetLotesDAE());
                    SelectedLote = LotesCollection.FirstOrDefault(i => i.ID.Equals(PesadaActual.LOTE));
                }                
            }            
        }

        public void CreateNewLoteAlgodon() {
            XX_OPM_BCI_LOTE maxLote = oracleDataManager.GetMaxLoteCurrentYear();            
            XX_OPM_BCI_LOTE newLote = new XX_OPM_BCI_LOTE();

            string Year = maxLote.ID.Substring(0, 2);
            string LoteCodigo = maxLote.ID.Substring(3, 3);

            newLote.ID = Year + "-" +
                (int.Parse(LoteCodigo) + 1).ToString("D3") + "-" +
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
                    PesadaActual.OBSERVACIONES = SelectedObervaciones;
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
                    PesadaActual.OBSERVACIONES = SelectedObervaciones;
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
                resetEditFields();
                resetTables();
                loadData();
            }          
            }

        public void resetEditFields()
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
            UpdateLotePanel();
            UpdateContratoPanel();

            AutoBascula = true;

            BtnBrutoIsEnabled = false;
            BtnTaraIsEnabled = false;
            BtnGuardarIsEnabled = false;            
        }

        public void resetTables()
        {
            SelectedPesadaPendiente = null;
        }

        public Boolean validate()
        {
            if (SelectedInventoryItem == null)
            {
                MessageBox.Show("Debe seleccionar un articulo");                
                return false;
            }
            if (SelectedTipoActividad == null)
            {
                MessageBox.Show("Debe seleccionar un tipo de actividad");                
                return false;
            }
            if (SelectedOrganisation == null)
            {
                MessageBox.Show("Debe seleccionar una organizacion");                
                return false;
            }
            if (SelectedPuntoOperacion == null)
            {
                MessageBox.Show("Debe seleccionar un " + PuntoOperacionLabel);                
                return false;
            }
            if (SelectedMatricula == null)
            {
                MessageBox.Show("Debe ingresar un Nro de Chapa");                
                return false;
            }
            if (SelectedEstab == null)
            {
                MessageBox.Show("Debe seleccionar un Establcimiento");                
                return false;
            }
            if (LoteVisibility == Visibility.Visible && SelectedLote == null)
            {
                MessageBox.Show("Este articulo requiere asignacion de un Lote");                
                return false;
            }            
            if (SelectedContrato == null && !SelectedEstab.ES_SOCIO.Equals("Si") && SelectedTipoActividad.Id == 1)
            {
                MessageBox.Show("Este proveedor debe tener un contrato habilitado");
                return false;
            }
            if (SelectedContrato != null && NotaRemisionVisibility == Visibility.Visible && (SelectedRemisionNro == null || SelectedRemisionPeso == null))
            {
                MessageBox.Show("El contrato seleccionado requiere de datos de la Nota de Remision");
                return false;
            }
            if ((PesoBruto == null && PesoTara == null) && NewPesada)
            {
                MessageBox.Show("Debe ingresar por lo menos un peso");
                return false;
            }
            if ((PesoBruto == null || PesoTara == null) && UpdatePesada)
            {
                MessageBox.Show("Falta ingresar un peso");
                return false;
            }
            return true;
        }

        private void serialStart()
        {
           /* serialPort.PortName = "COM1";
            serialPort.BaudRate = 9600;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.ReadTimeout = 400;
            serialPort.ReadBufferSize = 64;
            serialPort.Open();
            serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(SerialPortRecieve);
    */    
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
      //          PesoActual = int.Parse(serialPort.ReadLine().Substring(0, 4).Trim());
            }
            catch (Exception ex)
            {
                PesoActual = -1;
            }

        }

        private void showError(Exception ex)
        {
            StatusColor = new SolidColorBrush(Colors.Red);
            ActualException = ex;
            ErrorLinkVisibility = Visibility.Visible;
        }

    }
}
