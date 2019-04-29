using Dapper.Contrib.Extensions;
using System;
using System.ComponentModel;

namespace BCI.Models
{
    [Table("XX.X_OPM_BCI_PESADAS_ALL")]
    public class XX_OPM_BCI_PESADAS_ALL : INotifyPropertyChanged
    {
        [ExplicitKey]
        public int PESADA_ID { get; set; }
        public int ORG_ID { get; set; }
        public long ORGANIZATION_ID { get; set; }
        public XX_OPM_BCI_ORGS_COMPLEJO Organisation { get; set; }
        public long TIPO_ACTIVIDAD { get; set; }
        public XX_OPM_BCI_TIPO_ACTIVIDAD TipoActividad { get; set; }
        public long INVENTORY_ITEM_ID { get; set; }
        public XX_OPM_BCI_ITEMS_V InventoryItem { get; set; }
        public string PUNTO_DESCARGA { get; set; }
        public XX_OPM_BCI_PUNTO_OPERACION PuntoOperacion { get; set; }
        public string ESTABLECIMIENTO { get; set; }
        public XX_OPM_BCI_ESTAB Establecimiento { get; set; }
        public string OBSERVACIONES { get; set; }
        public int NRO_BASCULA { get; set; }
        public string LOTE { get; set; }        
        public string CONTRATO { get; set; }
        public XX_OPM_BCI_CONTRATOS_V Contrato { get; set; }
        public string MATRICULA { get; set; }        
        public int? PESO_BRUTO { get; set; }
        public char? MODO_PESO_BRUTO { get; set; }
        public DateTime? FECHA_PESO_BRUTO { get; set; }
        public int? PESO_TARA { get; set; }
        public char? MODO_PESO_TARA { get; set; }
        public DateTime? FECHA_PESO_TARA { get; set; }
        public char? CANCELADO { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime LAST_UPDATE_DATE { get; set; }
        public int LAST_UPDATED_BY { get; set; }
        public string ESTADO { get; set; }
        public string DISPOSICION { get; set; }

        public XX_OPM_BCI_PESADAS_ALL()
        {
            ORG_ID = 82;
            CREATED_BY = 3;
            CREATION_DATE = DateTime.Now;
            LAST_UPDATED_BY = 3;
            LAST_UPDATE_DATE = DateTime.Now;
        }

        public DateTime? EntryDate
        {
            get
            {
                if (FECHA_PESO_BRUTO != null && FECHA_PESO_TARA != null)
                {
                    if (FECHA_PESO_BRUTO > FECHA_PESO_TARA)
                    {
                        return FECHA_PESO_TARA;
                    }
                    else
                    {
                        return FECHA_PESO_BRUTO;
                    }
                }
                else if (FECHA_PESO_BRUTO != null && FECHA_PESO_TARA == null)
                {
                    return FECHA_PESO_BRUTO;
                }
                else if (FECHA_PESO_BRUTO == null && FECHA_PESO_TARA != null)
                {
                    return FECHA_PESO_TARA;
                }
                else
                {
                    return null;
                }  
            }
        }

        public DateTime? ExitDate
        {
            get{
                if (FECHA_PESO_BRUTO != null && FECHA_PESO_TARA != null)
                {
                    if (FECHA_PESO_BRUTO > FECHA_PESO_TARA)
                    {
                        return FECHA_PESO_TARA;
                    }
                    else
                    {
                        return FECHA_PESO_BRUTO;
                    }
                }            
                else
                {
                return null;
                }
            }
        }        

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
