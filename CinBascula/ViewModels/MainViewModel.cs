using Caliburn.Micro;
using CinBascula.Models;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows;

namespace CinBascula
{
    public class MainViewModel : PropertyChangedBase
    {

        SerialPort serialPort = new SerialPort();
        List<Articulo> ArticuloList = new List<Articulo>();
        List<Organizacion> organizacionList = new List<Organizacion>();
        List<PuntoDeDescarga> puntoDeDescargaList = new List<PuntoDeDescarga>();
        List<PuntoDeDescarga> puntoDeDescargaFilteredList = new List<PuntoDeDescarga>();
        List<TipoActividad> tipoActividadList = new List<TipoActividad>();

        public MainViewModel()
        {            
            organizacionList.Add(new Organizacion("DAE", "Palo Santo"));
            organizacionList.Add(new Organizacion("DAL", "Algodon"));
            organizacionList.Add(new Organizacion("FAA", "Balanceados"));
            organizacionList.Add(new Organizacion("FAV", "Cartamo/Mani"));
            organizacionList.Add(new Organizacion("PSE", "Sesamo"));

            puntoDeDescargaList.Add(new PuntoDeDescarga(1, "Palo Santo 1", organizacionList.ElementAt(0)));
            puntoDeDescargaList.Add(new PuntoDeDescarga(2, "Algodon", organizacionList.ElementAt(1)));
            puntoDeDescargaList.Add(new PuntoDeDescarga(3, "Balanceados 1", organizacionList.ElementAt(2)));
            puntoDeDescargaList.Add(new PuntoDeDescarga(4, "Balanceados 2", organizacionList.ElementAt(2)));
            puntoDeDescargaList.Add(new PuntoDeDescarga(5, "Balanceados 3", organizacionList.ElementAt(2)));
            puntoDeDescargaList.Add(new PuntoDeDescarga(6, "Cartamo/Mani", organizacionList.ElementAt(3)));
            puntoDeDescargaList.Add(new PuntoDeDescarga(7, "Sesamo", organizacionList.ElementAt(4)));

            tipoActividadList.Add(new TipoActividad(1, "Compra", "Proveedor"));
            tipoActividadList.Add(new TipoActividad(2, "Venta", "Cliente"));
            tipoActividadList.Add(new TipoActividad(3, "Servicio", "Responsable"));
            tipoActividadList.Add(new TipoActividad(4, "Servicio Interno", "Responsable"));
        }

        Organizacion getById(string id)
        {
            return organizacionList.First(x => x.Id.Equals(id));
        }

        public IReadOnlyList<TipoActividad> TipoActividadList
        {
            get { return tipoActividadList; }
        }

        TipoActividad selectedTipoActividad;
        public TipoActividad SelectedTipoActividad
        {
            get { return selectedTipoActividad; }
            set {
                selectedTipoActividad = value;
                TipoEstablecimiento = value.TipoEstablecimiento;                
                NotifyOfPropertyChange(() => SelectedTipoActividad);                
            }
        }

        public IReadOnlyList<Organizacion> OrganizacionList
        {
            get { return organizacionList; }
        }

        Organizacion selectedOrganizacion;
        public Organizacion SelectedOrganizacion
        {
            get { return selectedOrganizacion; }
            set
            {
                selectedOrganizacion = value;                
                NotifyOfPropertyChange(() => SelectedOrganizacion);
                NotifyOfPropertyChange(() => PuntoDeDescargaFilteredList);
            }
        }

        string tipoEstablecimiento;
        public string TipoEstablecimiento {
            get
            {
                return tipoEstablecimiento;
            }
            set
            {
                tipoEstablecimiento = SelectedTipoActividad.TipoEstablecimiento;
                NotifyOfPropertyChange(() => TipoEstablecimiento);
            }
        }

        public IReadOnlyList<PuntoDeDescarga> PuntoDeDescargaFilteredList
        {
            get { return puntoDeDescargaList.Where(x => x.Organizacion.Equals(SelectedOrganizacion)).ToList(); }                    
    }

        PuntoDeDescarga selectedPuntoDeDescarga;
        public PuntoDeDescarga SelectedPuntoDeDescarga
        {
            get
            {
                return selectedPuntoDeDescarga;
            }
            set
            {
                selectedPuntoDeDescarga = value;
                NotifyOfPropertyChange(() => SelectedPuntoDeDescarga);
            }
        }

        Visibility visibilityLote = Visibility.Hidden;
        Visibility VisibilityLote
        {
            get
            {
                return visibilityLote;
            }
            set
            {
                visibilityLote = value;
                NotifyOfPropertyChange(() => VisibilityLote);
            }
        }

        Articulo selectedArticulo;
        Articulo SelectedArticulo
        {
            get
            {
                return selectedArticulo;
            }
            set
            {
                selectedArticulo = value;
                NotifyOfPropertyChange(() => SelectedArticulo);
            }
        }

        bool autoBascula;
        bool AutoBascula{
            get
        {
                return autoBascula;
        }
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
                NotifyOfPropertyChange(() => AutoBascula);
    }
}


    }
}
