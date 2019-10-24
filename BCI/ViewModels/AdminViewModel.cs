using BCI.Models;
using RJCP.IO.Ports;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace BCI.ViewModels
{
    public class AdminViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void NotificationEventHandler(object sender, string msg, bool isError);
        public event NotificationEventHandler NotificationEvent;

        public XX_OPM_BCI_PESADAS_ALL SelectedPesada { get; set; }

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
        public ObservableCollection<XX_OPM_BCI_PESADAS_ALL> PesadasCerradasCollection { get; set; }
        public ICollectionView PesadasCerradasView { get; set; }
        public bool isLoading { get; set; }
        public Visibility ErrorLinkVisibility { get; set; }
        public Exception ActualException { get; set; }
        public Brush StatusColor { get; set; }

        public AdminViewModel()
        {
            try
            {
                isLoading = true;
                resetTables();
                loadData();
                isLoading = false;
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
                SelectedPesada = null;
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
                UpdatePesadasCerradasDatagrid();
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
                    switch (SelectedTipoActividad.Id)
                    {
                        case 1L:
                            OrganisationVisibility = Visibility.Visible;
                            EstablecimientoLabel = "Proveedor";
                            EstablecimientoVisibility = Visibility.Visible;
                            EstabsCollection = EstabsAPCollection;                           
                            break;
                        case 2L:
                            OrganisationVisibility = Visibility.Visible;
                            PuntoOperacionLabel = "Punto de Carga";
                            PuntoOperacionVisibility = Visibility.Visible;
                            EstablecimientoLabel = "Cliente";
                            EstablecimientoVisibility = Visibility.Visible;
                            EstabsCollection = EstabsARCollection;                            
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

        public void SelectedPesadaChanged()
        {
            try
            {
                if (!isLoading && SelectedPesada != null)
                {
                    resetEditFields();
                    //Task.WaitAll(Task.Run(async () =>
                    //{
                    SelectedPesada = oracleDataManager.GetPesadaByID(SelectedPesada.PESADA_ID);
                    //}));
                   

                    SelectedInventoryItem = InventoryItemsCollection.FirstOrDefault(i => i.INVENTORY_ITEM_ID.Equals(SelectedPesada.INVENTORY_ITEM_ID));
                    //InventoryItemsView.MoveCurrentTo(oracleDataManager.GetInventoryItemById(SelectedPesada.INVENTORY_ITEM_ID));
                    //SelectedInventoryItem = oracleDataManager.GetInventoryItemById(SelectedPesada.INVENTORY_ITEM_ID);
                    SelectedTipoActividad = TiposActividadCollection.FirstOrDefault(i => i.Id.Equals(SelectedPesada.TIPO_ACTIVIDAD));
                    SelectedOrganisation = OrganisationsCollection.FirstOrDefault(i => i.Id.Equals(SelectedPesada.ORGANIZATION_ID));

                    SelectedMatricula = SelectedPesada.MATRICULA;
                    if (SelectedTipoActividad.Id == 2)
                    {
                        SelectedPuntoOperacion = PuntosOperacionCollection.FirstOrDefault(i => i.Id.Equals(SelectedPesada.PUNTO_DESCARGA));
                        SelectedEstab = EstabsARCollection.FirstOrDefault(i => i.Id.Equals(SelectedPesada.ESTABLECIMIENTO));
                    }
                    else
                    {
                        SelectedPuntoOperacion = PuntosOperacionCollection.FirstOrDefault(i => i.Id.Equals(SelectedPesada.PUNTO_DESCARGA));
                        SelectedEstab = EstabsAPCollection.FirstOrDefault(i => i.Id.Equals(SelectedPesada.ESTABLECIMIENTO));
                    }
                    if (SelectedPesada.CONTRATO != null)
                    {
                        SelectedContrato = ContratosCollection.FirstOrDefault(i => i.NRO_CONTRATO.Equals(SelectedPesada.CONTRATO));
                    }
                    SelectedRemisionNro = SelectedPesada.NRO_NOTA_REMISION;
                    SelectedRemisionPeso = SelectedPesada.PESO_ORIGEN;
                    if (SelectedPesada.LOTE != null)
                    {
                        UpdateLotePanel();
                    }
                    SelectedObervaciones = SelectedPesada.OBSERVACIONES;                    
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
                        if (SelectedPesada.CONTRATO != null)
                        {
                            SelectedContrato = ContratosCollection.FirstOrDefault(i => i.NRO_CONTRATO.Equals(SelectedPesada.CONTRATO));
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
                        SelectedLote = LotesCollection.FirstOrDefault(i => i.ID.Equals(SelectedPesada.LOTE));
                    }
                    else if (SelectedInventoryItem.CODIGO_ITEM.Equals("050.001895"))
                    {
                        LoteVisibility = Visibility.Visible;
                        LotesCollection = new ObservableCollection<XX_OPM_BCI_LOTE>(oracleDataManager.GetLotesDAE());
                        SelectedLote = LotesCollection.FirstOrDefault(i => i.ID.Equals(SelectedPesada.LOTE));
                    }
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

        private void showNotification(string msg, bool isError)
        {
            if (NotificationEvent != null)
            {
                NotificationEvent.Invoke(this, msg, isError);
            }
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
