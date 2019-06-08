using BCI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace BCI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void NotificationEventHandler(object sender, string msg, bool isError);
        public event NotificationEventHandler NotificationEvent;

        private void showNotification(string msg, bool isError)
        {
            if (NotificationEvent != null)
            {
                NotificationEvent.Invoke(this, msg, isError);
            }
        }

        public XX_OPM_BCI_PESADAS_ALL PesadaActual;
        public bool NewPesada { get; set; }
        public bool UpdatePesada { get; set; }
        public bool NewOrUpdatePesada { get; set; }
        public bool isLoading { get; set; }

        public int? PesoActual { get; set; }
        public int? PesoBruto { get; set; }
        public int? PesoTara { get; set; }

        private SerialPort serialPort;
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
        public Visibility ContratoVisibility { get; set; }
        public XX_OPM_BCI_CONTRATOS_V SelectedContrato { get; set; }
        public ObservableCollection<XX_OPM_BCI_CONTRATOS_V> ContratosCollection { get; set; }
        public Visibility NotaRemisionVisibility { get; set; }
        public String SelectedRemisionNro { get; set; }
        public int? SelectedRemisionPeso { get; set; }
        public string SelectedObervaciones { get; set; }
        public ObservableCollection<XX_OPM_BCI_PESADAS_ALL> PesadasPendientesCollection { get; set; }
        public XX_OPM_BCI_PESADAS_ALL SelectedPesadaPendiente { get; set; }
        public ICollectionView PesadasPendientesView { get; set; }
        public ObservableCollection<XX_OPM_BCI_PESADAS_ALL> PesadasCerradasCollection { get; set; }
        public ICollectionView PesadasCerradasView { get; set; }
        private bool autoBascula;
        public bool AutoBascula
        {
            get => autoBascula;
            set
            {
                autoBascula = value;
                if (autoBascula)
                {
                    serialStart();
                }
                else
                {
                    serialStop();
                }
            }
        }
        public bool ManualBascula { get { return !AutoBascula; } }
        public bool BtnBrutoIsEnabled { get; set; }
        public bool BtnTaraIsEnabled { get; set; }
        public bool BtnGuardarIsEnabled { get; set; }

        public Visibility ErrorLinkVisibility { get; set; }
        public Exception ActualException { get; set; }
        public Brush StatusColor { get; set; }

        private System.Timers.Timer timer = new System.Timers.Timer();

        private TicketPrinterManager ticketPrinterManager = new TicketPrinterManager();

        bool SerialPortPendingClose = false;

        public MainViewModel()
        {
            try
            {
                isLoading = true;
                resetEditFields();
                resetTables();
                loadData();
                timer.Interval = 10000;
                timer.Elapsed += timer_Elapsed;
                timer.Start();
                isLoading = false;
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (SelectedInventoryItem == null)
                {
                    UpdatePesadasPendientesDatagrid();
                }
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void loadData()
        {
            try
            {
                loadLOV();
                loadDatagrid();
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void loadLOV()
        {
            try
            {
                InventoryItemsCollection = new ObservableCollection<XX_OPM_BCI_ITEMS_V>(oracleDataManager.GetInventoryItemList());
                InventoryItemsView = new CollectionViewSource { Source = InventoryItemsCollection.Where(p => p.ORGANIZATION_ID != 0 && p == InventoryItemsCollection.First(i => i.CODIGO_ITEM == p.CODIGO_ITEM)) }.View;
                TiposActividadCollection = new ObservableCollection<XX_OPM_BCI_TIPO_ACTIVIDAD>(oracleDataManager.GetTipoActividadList());
                OrganisationsCollection = new ObservableCollection<XX_OPM_BCI_ORGS_COMPLEJO>(oracleDataManager.GetOrgsComplejoList());
                PuntosDescargaCollection = new ObservableCollection<XX_OPM_BCI_PUNTO_OPERACION>(oracleDataManager.GetPuntoDescargaList());
                PuntosCargaCollection = new ObservableCollection<XX_OPM_BCI_PUNTO_OPERACION>(oracleDataManager.GetPuntoCargaList());
                ContratosCollection = new ObservableCollection<XX_OPM_BCI_CONTRATOS_V>(oracleDataManager.GetContratosList());
                EstabsAPCollection = new ObservableCollection<XX_OPM_BCI_ESTAB>(oracleDataManager.GetEstabAPList());
                EstabsARCollection = new ObservableCollection<XX_OPM_BCI_ESTAB>(oracleDataManager.GetEstabARList());
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void loadDatagrid()
        {
            try
            {
                UpdatePesadasPendientesDatagrid();
                UpdatePesadasCerradasDatagrid();
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        private void UpdatePesadasPendientesDatagrid()
        {
            try
            {
                PesadasPendientesCollection = new ObservableCollection<XX_OPM_BCI_PESADAS_ALL>(oracleDataManager.GetPesadasPendientes());
                PesadasPendientesCollection.ToList().ForEach(x => completeDataPesada(x));
                PesadasPendientesCollection.ToList().ForEach(x =>
                {
                    if (x.AUTORIZ_REQ_ID == null)
                        imprimirAutorizacion(x);
                });
                PesadasPendientesView = new CollectionViewSource { Source = PesadasPendientesCollection }.View;
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        private void UpdatePesadasCerradasDatagrid()
        {
            try
            {
                PesadasCerradasCollection = new ObservableCollection<XX_OPM_BCI_PESADAS_ALL>(oracleDataManager.GetPesadasCerradas());
                PesadasCerradasCollection.ToList().ForEach(x => completeDataPesada(x));

                PesadasCerradasView = new CollectionViewSource { Source = PesadasCerradasCollection }.View;
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        /*public void UpdateInventoryItemsPanel()
        {
            try
            {
                                InventoryItemsCollection = new ObservableCollection<XX_OPM_BCI_ITEMS_V>(await oracleDataManager.GetInventoryItemList());
                                InventoryItemsView = new CollectionViewSource { Source = InventoryItemsCollection.Where(p => p.ORGANIZATION_ID != 0 && p == InventoryItemsCollection.First(i => i.CODIGO_ITEM == p.CODIGO_ITEM)) }.View;
                                TiposActividadCollection = new ObservableCollection<XX_OPM_BCI_TIPO_ACTIVIDAD>(await oracleDataManager.GetTipoActividadList());
                                OrganisationsCollection = new ObservableCollection<XX_OPM_BCI_ORGS_COMPLEJO>(await oracleDataManager.GetOrgsComplejoList());
                                PuntosDescargaCollection = new ObservableCollection<XX_OPM_BCI_PUNTO_OPERACION>(await oracleDataManager.GetPuntoDescargaList());
                                PuntosCargaCollection = new ObservableCollection<XX_OPM_BCI_PUNTO_OPERACION>(await oracleDataManager.GetPuntoCargaList());
                                EstabsAPCollection = new ObservableCollection<XX_OPM_BCI_ESTAB>(await oracleDataManager.GetEstabAPList());
                                EstabsARCollection = new ObservableCollection<XX_OPM_BCI_ESTAB>(await oracleDataManager.GetEstabARList());             
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }*/

        public void SelectedInventoryItemChanged()
        {
            try
            {
                UpdateTiposActividadPanel();
                UpdateOrganisationPanel();
                UpdateLotePanel();
                UpdateContratoPanel();
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        private void UpdateTiposActividadPanel()
        {
            try
            {
                if (SelectedInventoryItem != null)
                {
                    TiposActividadView = new CollectionViewSource
                    {
                        Source = TiposActividadCollection
                        .Where(p => InventoryItemsCollection
                            .Where(i => i.INVENTORY_ITEM_ID == SelectedInventoryItem.INVENTORY_ITEM_ID)
                            .Any(a => a.TIPO_ACTIVIDAD == p.Id))
                    }.View;
                    SelectedTipoActividad = null; //poner en null para disparar cambio de habilitacion de botones Bruto/Tara
                    TiposActividadView.MoveCurrentToFirst();
                    SelectedTipoActividad = (XX_OPM_BCI_TIPO_ACTIVIDAD)TiposActividadView.CurrentItem;
                }
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void SelectedTipoActividadChanged()
        {
            try
            {
                if (SelectedTipoActividad != null)
                {
                    BtnTaraIsEnabled = true;
                    BtnBrutoIsEnabled = true;
                    switch (SelectedTipoActividad.Id)
                    {
                        case 1L:
                            OrganisationVisibility = Visibility.Visible;
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
                                if (SelectedInventoryItem.DESCRIPCION_ITEM.ToUpper().Contains("ENTRADA"))
                                {
                                    BtnBrutoIsEnabled = true; //--- para venta de servicios
                                    BtnTaraIsEnabled = false; //--- para venta de servicios
                                }
                            }
                            break;
                        case 3L:
                            OrganisationVisibility = Visibility.Collapsed;
                            EstablecimientoVisibility = Visibility.Collapsed;
                            EstabsCollection = EstabServiciosCollection;
                            PuntoOperacionVisibility = Visibility.Collapsed;
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
                    UpdatePuntoOperacionPanel();
                }
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        private void UpdateOrganisationPanel()
        {
            try
            {
                if (SelectedInventoryItem != null)
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
                    UpdatePuntoOperacionPanel();
                }
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void SelectedOrganisationChanged()
        {
            try
            {
                UpdatePuntoOperacionPanel();
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }
        private void UpdatePuntoOperacionPanel()
        {
            try
            {
                if (SelectedTipoActividad != null && SelectedOrganisation != null)
                {
                    switch (SelectedTipoActividad.Id)
                    {
                        case 2L:
                            PuntoOperacionLabel = "Punto de Carga";
                            PuntoOperacionVisibility = Visibility.Visible;
                            PuntosOperacionCollection = new ObservableCollection<XX_OPM_BCI_PUNTO_OPERACION>(PuntosCargaCollection.Where(o => o.Tag.Equals(SelectedOrganisation.Tag)));
                            break;
                        case 3L:
                            PuntoOperacionVisibility = Visibility.Collapsed;
                            PuntosOperacionCollection = new ObservableCollection<XX_OPM_BCI_PUNTO_OPERACION>(PuntosDescargaCollection.Where(o => o.Tag.Equals(SelectedOrganisation.Tag)));
                            break;
                        default:
                            PuntoOperacionLabel = "Punto de Descarga";
                            PuntoOperacionVisibility = Visibility.Visible;
                            PuntosOperacionCollection = new ObservableCollection<XX_OPM_BCI_PUNTO_OPERACION>(PuntosDescargaCollection.Where(o => o.Tag.Equals(SelectedOrganisation.Tag)));
                            break;
                    }
                    SelectedPuntoOperacion = PuntosOperacionCollection.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }



        public void UpdateEstablecimientoPanel()
        {
            try
            {
                EstabsAPCollection = new ObservableCollection<XX_OPM_BCI_ESTAB>(oracleDataManager.GetEstabAPList());
                EstabsARCollection = new ObservableCollection<XX_OPM_BCI_ESTAB>(oracleDataManager.GetEstabARList());
                SelectedTipoActividadChanged();
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void SelectedEstabChanged()
        {
            try
            {
                UpdateLotePanel();
                UpdateContratoPanel();
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void CreateNewPesada()
        {
            try
            {
                resetEditFields();
                PesadaActual = new XX_OPM_BCI_PESADAS_ALL();
                NewPesada = true;
                BtnBrutoIsEnabled = true;
                BtnTaraIsEnabled = true;
                BtnGuardarIsEnabled = true;
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void SelectedPesadaPendientesChanged()
        {
            try
            {
                if (!isLoading && SelectedPesadaPendiente != null)
                {
                    resetEditFields();
                    //Task.WaitAll(Task.Run(async () =>
                    //{
                    PesadaActual = oracleDataManager.GetPesadaByID(SelectedPesadaPendiente.PESADA_ID);
                    //}));
                    if (!PesadaActual.ESTADO.Equals("Completo"))
                    {
                        //MessageBox.Show("Aun no hay datos de calidad registradas.");      
                        showNotification("Aun no hay datos de calidad registradas.", false);
                        return;
                    }

                    SelectedInventoryItem = InventoryItemsCollection.FirstOrDefault(i => i.INVENTORY_ITEM_ID.Equals(PesadaActual.INVENTORY_ITEM_ID));
                    //InventoryItemsView.MoveCurrentTo(oracleDataManager.GetInventoryItemById(PesadaActual.INVENTORY_ITEM_ID));
                    //SelectedInventoryItem = oracleDataManager.GetInventoryItemById(PesadaActual.INVENTORY_ITEM_ID);
                    SelectedTipoActividad = TiposActividadCollection.FirstOrDefault(i => i.Id.Equals(PesadaActual.TIPO_ACTIVIDAD));
                    SelectedOrganisation = OrganisationsCollection.FirstOrDefault(i => i.Id.Equals(PesadaActual.ORGANIZATION_ID));

                    SelectedMatricula = PesadaActual.MATRICULA;
                    if (SelectedTipoActividad.Id == 2)
                    {
                        SelectedPuntoOperacion = PuntosOperacionCollection.FirstOrDefault(i => i.Id.Equals(PesadaActual.PUNTO_DESCARGA));
                        SelectedEstab = EstabsARCollection.FirstOrDefault(i => i.Id.Equals(PesadaActual.ESTABLECIMIENTO));
                    }
                    else
                    {
                        SelectedPuntoOperacion = PuntosOperacionCollection.FirstOrDefault(i => i.Id.Equals(PesadaActual.PUNTO_DESCARGA));
                        SelectedEstab = EstabsAPCollection.FirstOrDefault(i => i.Id.Equals(PesadaActual.ESTABLECIMIENTO));
                    }
                    if (PesadaActual.CONTRATO != null)
                    {
                        SelectedContrato = ContratosCollection.FirstOrDefault(i => i.NRO_CONTRATO.Equals(PesadaActual.CONTRATO));
                    }
                    SelectedRemisionNro = PesadaActual.NRO_NOTA_REMISION;
                    SelectedRemisionPeso = PesadaActual.PESO_ORIGEN;
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
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        private void UpdateContratoPanel()
        {
            try
            {
                SelectedContrato = null;
                /*Task.WaitAll(Task.Run(async () =>
                {
                    ContratosCollection = new ObservableCollection<XX_OPM_BCI_CONTRATOS_V>(await oracleDataManager.GetContratosList());
                }));*/
                ContratoVisibility = Visibility.Collapsed;
                if (SelectedInventoryItem != null && SelectedEstab != null && SelectedTipoActividad.Id == 1)
                {
                    //                    Task.WaitAll(Task.Run(async () =>
                    //                    {
                    ContratosCollection = new ObservableCollection<XX_OPM_BCI_CONTRATOS_V>(oracleDataManager.GetContratoByEstablecimientoAndItem(SelectedEstab, SelectedInventoryItem));

                    if (ContratosCollection.Count > 0)
                    {
                        ContratoVisibility = Visibility.Visible;
                        if (PesadaActual.CONTRATO != null)
                        {
                            SelectedContrato = ContratosCollection.FirstOrDefault(i => i.NRO_CONTRATO.Equals(PesadaActual.CONTRATO));
                        }
                        else
                        {
                            SelectedContrato = ContratosCollection.ElementAtOrDefault(0);
                        }
                    }
                }
                //                    }));
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void SelectedContratoChanged()
        {
            try
            {
                UpdateNotaRemisionPanel();
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        private void UpdateNotaRemisionPanel()
        {
            try
            {
                NotaRemisionVisibility = Visibility.Collapsed;
                if (SelectedContrato != null && SelectedContrato.PESO_ORIGEN == "Y")
                {
                    NotaRemisionVisibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        private void UpdateLotePanel()
        {
            try
            {
                SelectedLote = null;
                LoteVisibility = Visibility.Collapsed;
                NewLoteBtnVisibility = Visibility.Collapsed;
                LotesCollection = new ObservableCollection<XX_OPM_BCI_LOTE>();
                if (SelectedInventoryItem != null && SelectedEstab != null && SelectedTipoActividad.Id == 1)
                {
                    if (SelectedInventoryItem.CODIGO_ITEM.Equals("050.002198"))
                    {
                        LoteVisibility = Visibility.Visible;
                        NewLoteBtnVisibility = Visibility.Visible;
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
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void CreateNewLoteAlgodon()
        {
            try
            {
                XX_OPM_BCI_LOTE maxLote = oracleDataManager.GetMaxLoteCurrentYear();                

                string Year = maxLote.ID.Substring(0, 2);
                string LoteCodigo = maxLote.ID.Substring(3, 3);

                XX_OPM_BCI_LOTE newLote = new XX_OPM_BCI_LOTE(Year + "-" +
                    (int.Parse(LoteCodigo) + 1).ToString("D3") + "-" +
                    SelectedEstab.Id);

                /*newLote.ID = Year + "-" +
                    (int.Parse(LoteCodigo) + 1).ToString("D3") + "-" +
                    SelectedEstab.Id;*/
                LotesCollection.Add(newLote);
                SelectedLote = newLote;
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void Save()
        {
            try
            {
                if (PesadaActual != null)
                {
                    PesadaActual.INVENTORY_ITEM_ID = SelectedInventoryItem.INVENTORY_ITEM_ID;
                    PesadaActual.TIPO_ACTIVIDAD = SelectedTipoActividad.Id;
                    if (SelectedOrganisation != null)
                    {
                        PesadaActual.ORGANIZATION_ID = SelectedOrganisation.Id;
                    }
                    if (SelectedPuntoOperacion != null)
                    {
                        PesadaActual.PUNTO_DESCARGA = SelectedPuntoOperacion.Id.ToString();
                    }
                    PesadaActual.MATRICULA = SelectedMatricula;
                    if (SelectedEstab != null)
                    {
                        PesadaActual.ESTABLECIMIENTO = SelectedEstab.Id;
                    }
                    if (SelectedContrato != null)
                    {
                        PesadaActual.CONTRATO = SelectedContrato.NRO_CONTRATO;
                    }
                    else
                    {
                        PesadaActual.CONTRATO = null;
                    }
                    if (SelectedRemisionNro != null && !SelectedRemisionNro.Replace("-", "").Trim().Equals(""))
                    {
                        PesadaActual.NRO_NOTA_REMISION = SelectedRemisionNro;
                    }
                    else
                    {
                        PesadaActual.NRO_NOTA_REMISION = null;
                    }
                    if (SelectedRemisionPeso != null)
                    {
                        PesadaActual.PESO_ORIGEN = SelectedRemisionPeso;
                    }
                    else
                    {
                        PesadaActual.PESO_ORIGEN = null;
                    }
                    if (SelectedLote != null)
                    {
                        PesadaActual.LOTE = SelectedLote.ID;
                    }
                    else
                    {
                        PesadaActual.LOTE = null;
                    }
                    PesadaActual.OBSERVACIONES = SelectedObervaciones;

                    if (NewPesada)
                    {
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
                        PesadaActual.PESADA_ID = oracleDataManager.insertNewPesada(PesadaActual);
                        try
                        {
                            if (PesadaActual.TIPO_ACTIVIDAD == 1 || PesadaActual.TIPO_ACTIVIDAD == 2)
                            {
                                ticketPrinterManager.imprimirTicketRecMuestra(completeDataPesada(PesadaActual));
                            }
                            else if ((PesadaActual.TIPO_ACTIVIDAD == 3 || PesadaActual.TIPO_ACTIVIDAD == 4) && PesoBruto != null && PesoTara != null)
                            {
                                imprimirTicket(PesadaActual);
                            }
                            if (PesadaActual.TIPO_ACTIVIDAD == 2)
                            {
                                //imprimirAutorizacion(completeDataPesada(PesadaActual));
                            }
                        }
                        catch (Exception ex)
                        {
                            showError(ex);
                        }
                    }
                    else if (UpdatePesada)
                    {
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
                        try
                        {
                            if (PesadaActual.TIPO_ACTIVIDAD == 1)
                            {
                                imprimirCertificado(PesadaActual);
                            }
                            else
                            {
                                imprimirTicket(PesadaActual);
                            }
                        }
                        catch (Exception ex)
                        {
                            showError(ex);
                        }
                    }
                    resetEditFields();
                    resetTables();
                    //loadData();
                    UpdatePesadasPendientesDatagrid();
                    UpdatePesadasCerradasDatagrid();
                }
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void resetEditFields()
        {
            try
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
                SelectedObervaciones = null;
                SelectedRemisionNro = null;
                SelectedRemisionPeso = null;
                UpdateLotePanel();
                //UpdateContratoPanel();
                UpdateNotaRemisionPanel();

                //AutoBascula = true;

                BtnBrutoIsEnabled = false;
                BtnTaraIsEnabled = false;
                BtnGuardarIsEnabled = false;
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void resetTables()
        {
            try
            {
                SelectedPesadaPendiente = null;
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public Boolean validate()
        {
            try
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
                if (SelectedOrganisation == null && SelectedTipoActividad.Id != 3)
                {
                    MessageBox.Show("Debe seleccionar una organizacion");
                    return false;
                }
                if (SelectedPuntoOperacion == null && SelectedTipoActividad.Id != 3)
                {
                    MessageBox.Show("Debe seleccionar un " + PuntoOperacionLabel);
                    return false;
                }
                if (SelectedMatricula == null || SelectedMatricula.Length < 7)
                {
                    MessageBox.Show("Debe ingresar un Nro de Chapa valido");
                    return false;
                }
                if (SelectedEstab == null && SelectedTipoActividad.Id != 3)
                {
                    MessageBox.Show("Debe seleccionar un Establcimiento");
                    return false;
                }
                if (LoteVisibility == Visibility.Visible && SelectedLote == null)
                {
                    MessageBox.Show("Este articulo requiere asignacion de un Lote");
                    return false;
                }
                if (SelectedContrato == null && SelectedTipoActividad.Id == 1 && !SelectedEstab.ES_SOCIO.Equals("Si"))
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
            catch (Exception ex)
            {
                showError(ex);
                return false;
            }
        }

        private XX_OPM_BCI_PESADAS_ALL completeDataPesada(XX_OPM_BCI_PESADAS_ALL pesada)
        {
            pesada.InventoryItem = InventoryItemsCollection.FirstOrDefault(c => c.INVENTORY_ITEM_ID.Equals(pesada.INVENTORY_ITEM_ID));
            pesada.TipoActividad = TiposActividadCollection.FirstOrDefault(c => c.Id.Equals(pesada.TIPO_ACTIVIDAD));
            pesada.Organisation = OrganisationsCollection.FirstOrDefault(c => c.Id.Equals(pesada.ORGANIZATION_ID));

            if (pesada.TIPO_ACTIVIDAD == 2)
            {
                pesada.Establecimiento = EstabsARCollection.FirstOrDefault(c => c.Id.Equals(pesada.ESTABLECIMIENTO));
                pesada.PuntoOperacion = PuntosCargaCollection.FirstOrDefault(c => c.Id.Equals(pesada.PUNTO_DESCARGA));
            }
            else
            {
                pesada.Establecimiento = EstabsAPCollection.FirstOrDefault(c => c.Id.Equals(pesada.ESTABLECIMIENTO));
                pesada.PuntoOperacion = PuntosDescargaCollection.FirstOrDefault(c => c.Id.Equals(pesada.PUNTO_DESCARGA));
            }
            return pesada;
        }

        public void imprimirAutorizacion(XX_OPM_BCI_PESADAS_ALL pesada)
        {
            try
            {
                if (pesada.ESTADO.Equals("Completo"))
                {
                    string msg = oracleDataManager.imprimirAutorizacion(pesada);
                    if (msg.ToUpper().Contains("ERROR"))
                    {
                        showNotification(msg, true);
                    }
                }
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void imprimirCertificado(XX_OPM_BCI_PESADAS_ALL pesada)
        {
            try
            {
                string msg = oracleDataManager.imprimirCertificado(pesada);
                if (msg.ToUpper().Contains("ERROR"))
                {
                    showNotification(msg, true);
                }
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void imprimirTicket(XX_OPM_BCI_PESADAS_ALL pesada)
        {
            try
            {
                ticketPrinterManager.imprimirTicket(completeDataPesada(pesada));
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        private void serialStart()
        {
            try
            {                
               serialStop();
                
                serialPort = new SerialPort();
                serialPort.PortName = Properties.Settings.Default.SerialPort == "" ? "COM1" : Properties.Settings.Default.SerialPort;
                serialPort.BaudRate = 9600;
                serialPort.DataBits = 8;
                serialPort.Parity = Parity.None;
                serialPort.StopBits = StopBits.One;
                serialPort.ReadTimeout = 400;
                serialPort.ReadBufferSize = 64;
                serialPort.Open();
                serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(SerialPortRecieve);
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        public void serialStop()
        {
            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {                    
                SerialPortPendingClose = true;
                Thread.Sleep(serialPort.ReadTimeout);
                serialPort.DtrEnable = false;
                serialPort.RtsEnable = false;
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
                serialPort.Close();
                SerialPortPendingClose = false;
                //showNotification("Sistea en Manual", false);
                PesoActual = null;                    
                }
            }
            catch (Exception ex)
            {
                showError(ex);
            }
        }

        private void SerialPortRecieve(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (!SerialPortPendingClose)
                {
                    string reading = serialPort.ReadLine();
                    PesoActual = int.Parse(reading.Substring(3, reading.Length - 3).Trim());
                }
                else
                {
                    serialPort.DataReceived -= SerialPortRecieve;
                }
            }
            catch (Exception ex)
            {
                PesoActual = null;
                showError(ex);
            }
            //});                        
        }

        public void showError(Exception ex)
        {
            showNotification(ex.Message, true);
            //Application.Current.Dispatcher.Invoke(new Action(() =>
            //{
            StatusColor = new SolidColorBrush(Colors.Red);
            ActualException = ex;
            ErrorLinkVisibility = Visibility.Visible;
            //}));
        }

    }
}
