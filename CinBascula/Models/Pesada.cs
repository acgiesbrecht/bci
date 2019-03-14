using System;

namespace CinBascula.Models
{
    public sealed class XX_OPM_BCI_PESADAS_ALL
    {
        public int PESADA_ID { get; set; }
        public int ORG_ID { get; set; }
        public Organizacion ORGANIZATION_ID { get; set; }
        public TipoActividad TIPO_ACTIVIDAD { get; set; }
        public Articulo INVENTORY_ITEM_ID { get; set; }
        public PuntoDeDescarga PUNTO_DESCARGA { get; set; }
        public Establecimiento ESTABLECIMIENTO { get; set; }
        public string OBSERVACIONES { get; set; }
        public int NRO_BASCULA { get; set; }
        public string LOTE { get; set; }
        public string CONTRATO { get; set; }
        public string MATRICULA { get; set; }
        public int PESO_BRUTO { get; set; }
        public DateTime FECHA_PESO_BRUTO { get; set; }
        public int PESO_TARA { get; set; }
        public DateTime FECHA_PESO_TARA { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime LAST_UPDATE_DATE { get; set; }
        public int LAST_UPDATED_BY { get; set; }
    }
}
