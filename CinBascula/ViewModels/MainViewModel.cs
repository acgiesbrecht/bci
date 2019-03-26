using CinBascula.Models;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;

namespace CinBascula.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private SerialPort serialPort = new SerialPort();
        private OracleDataManager oracleDataManager = new OracleDataManager();

        public ObservableCollection<XX_OPM_BCI_ITEMS_V> InventoryItemsCollection;
        public ObservableCollection<XX_OPM_BCI_CONTRATOS_V> ContratosCollection;
        public ObservableCollection<XX_OPM_BCI_PESADAS_ALL> PesadasPendientesList;
        public ObservableCollection<XX_OPM_BCI_ORGS_COMPLEJO> OrganisationCollection = new ObservableCollection<XX_OPM_BCI_ORGS_COMPLEJO>();
        public ObservableCollection<XX_OPM_BCI_PUNTO_DESCARGA> PuntoDescargaCollection = new ObservableCollection<XX_OPM_BCI_PUNTO_DESCARGA>();
        public ObservableCollection<XX_OPM_BCI_TIPO_ACTIVIDAD> TipoActividadCollection = new ObservableCollection<XX_OPM_BCI_TIPO_ACTIVIDAD>();
        public ObservableCollection<XX_OPM_BCI_ESTAB> EstabAllCollection = new ObservableCollection<XX_OPM_BCI_ESTAB>();
        public ObservableCollection<XX_OPM_BCI_ESTAB> EstabAPCollection = new ObservableCollection<XX_OPM_BCI_ESTAB>();
        public ObservableCollection<XX_OPM_BCI_ESTAB> EstabARCollection = new ObservableCollection<XX_OPM_BCI_ESTAB>();
                        
        public IReadOnlyList<XX_OPM_BCI_PUNTO_DESCARGA> PuntoDeDescargaFilteredList;

        public MainViewModel()
        {

            loadLookUps();
            
            InventoryItemsCollection = new ObservableCollection<XX_OPM_BCI_ITEMS_V>(oracleDataManager.GetInventoryItemList());
            ContratosCollection = new ObservableCollection<XX_OPM_BCI_CONTRATOS_V>(oracleDataManager.GetContratosList());

            PesadasPendientesList = new ObservableCollection<XX_OPM_BCI_PESADAS_ALL>(oracleDataManager.GetPesadas());
            PesadasPendientesList.ToList().ForEach(x => x.InventoryItem = InventoryItemsCollection.FirstOrDefault(c => c.INVENTORY_ITEM_ID.Equals(x.INVENTORY_ITEM_ID)));
            PesadasPendientesList.ToList().ForEach(x => x.Organisation = OrganisationCollection.FirstOrDefault(c => c.Id.Equals(x.ORGANIZATION_ID)));
            PesadasPendientesList.ToList().ForEach(x => x.PuntoDescarga = PuntoDescargaCollection.FirstOrDefault(c => c.Id.Equals(x.PUNTO_DESCARGA)));
            PesadasPendientesList.ToList().ForEach(x => x.Establecimiento = EstabAllCollection.FirstOrDefault(c => c.Id.Equals(x.ESTABLECIMIENTO)));
            PesadasPendientesList.ToList().ForEach(x => x.Contrato = ContratosCollection.FirstOrDefault(c => c.NRO_CONTRATO.Equals(x.CONTRATO)));
                        
            visibilityLote = false;
            visibilityContrato = false;

            SetBruto = ReactiveCommand.Create(SetBrutoImpl);
            SetTara = ReactiveCommand.Create(SetTaraImpl);
            NewPesada = ReactiveCommand.Create(NewPesadaImpl);
            Guardar = ReactiveCommand.Create(GuardarImpl);
            /*this.WhenAnyValue(x => x.SelectedPesada.ORGANIZATION_ID)
                .Subscribe(PuntoDeDescargaFilteredList => PuntoDescargaCollection.Where(y => y.OrganisationTag.Equals(SelectedPesada.Organisation.ShortDescription)).ToList());
            
            var loader = PuntoDeDescargaCache.Connect()
                .Filter(x=>x.Organizacion.Equals(SelectedPesada.ORGANIZATION_ID))
                .Bind(out PuntoDeDescargaFilteredList).Subscribe();
                */
            Peso = 9999;            
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

        [Reactive] public XX_OPM_BCI_PESADAS_ALL SelectedPesada { get; set; }
        [Reactive] public bool visibilityLote { get; set; }
        [Reactive] public bool visibilityContrato { get; set; }

        private bool autoBascula;
        [Reactive] public bool AutoBascula
        {
            get => autoBascula;
            set
            {
                autoBascula = value;
                if (autoBascula)
                {
                    serialPort.PortName = "COM1";
                    serialPort.BaudRate = 9600;
                    serialPort.DataBits = 8;
                    serialPort.Parity = Parity.None;
                    serialPort.StopBits = StopBits.One;
                    serialPort.ReadTimeout = 400;
                    serialPort.ReadBufferSize = 64;
                    serialPort.Open();
                }
                else
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.Close();
                    }
                }
                this.RaiseAndSetIfChanged(ref autoBascula, value);                
            }
        }
        
        [Reactive] public int Peso { get; set; }                

        public ReactiveCommand<Unit, Unit> SetBruto { get; }
        public void SetBrutoImpl() {SelectedPesada.PESO_BRUTO = Peso;}

        public ReactiveCommand<Unit, Unit> SetTara { get; }
        public void SetTaraImpl()
        {SelectedPesada.PESO_TARA = Peso;}        

        public ReactiveCommand<Unit, Unit> NewPesada { get; }
        public void NewPesadaImpl()
        {SelectedPesada = new XX_OPM_BCI_PESADAS_ALL();}

        public ReactiveCommand<Unit, Unit> Guardar { get; }
        public void GuardarImpl()
        { Console.WriteLine(SelectedPesada.ORGANIZATION_ID); }
               
    }
}
