﻿using Dapper.Contrib.Extensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace CinBascula.Models
{
    [Table("XX.X_OPM_BCI_PESADAS_ALL")]
    public class XX_OPM_BCI_PESADAS_ALL : ReactiveObject
    {
        [ExplicitKey]
        [Reactive] public int PESADA_ID { get; set; }
        [Reactive] public int ORG_ID { get; set; }
        [Reactive] public long ORGANIZATION_ID { get; set; }
        [Reactive] public XX_OPM_BCI_ORGS_COMPLEJO Organisation { get; set; }
        [Reactive] public long TIPO_ACTIVIDAD { get; set; }
        [Reactive] public XX_OPM_BCI_TIPO_ACTIVIDAD TipoActividad { get; set; }
        [Reactive] public long INVENTORY_ITEM_ID { get; set; }
        [Reactive] public XX_OPM_BCI_ITEMS_V InventoryItem { get; set; }
        [Reactive] public string PUNTO_DESCARGA { get; set; }
        [Reactive] public XX_OPM_BCI_PUNTO_DESCARGA PuntoDescarga { get; set; }
        [Reactive] public string ESTABLECIMIENTO { get; set; }
        [Reactive] public XX_OPM_BCI_ESTAB Establecimiento { get; set; }
        [Reactive] public string OBSERVACIONES { get; set; }
        [Reactive] public int NRO_BASCULA { get; set; }
        [Reactive] public string LOTE { get; set; }
        [Reactive] public string CONTRATO { get; set; }
        [Reactive] public XX_OPM_BCI_CONTRATOS_V Contrato { get; set; }
        [Reactive] public string MATRICULA { get; set; }        
        [Reactive] public int? PESO_BRUTO { get; set; }
        [Reactive] public char? MODO_PESO_BRUTO { get; set; }
        [Reactive] public DateTime? FECHA_PESO_BRUTO { get; set; }
        [Reactive] public int? PESO_TARA { get; set; }
        [Reactive] public char? MODO_PESO_TARA { get; set; }
        [Reactive] public DateTime? FECHA_PESO_TARA { get; set; }
        [Reactive] public DateTime CREATION_DATE { get; set; }
        [Reactive] public int CREATED_BY { get; set; }
        [Reactive] public DateTime LAST_UPDATE_DATE { get; set; }
        [Reactive] public int LAST_UPDATED_BY { get; set; }

        public XX_OPM_BCI_PESADAS_ALL()
        {
            ORG_ID = 82;
            CREATED_BY = 3;
            LAST_UPDATED_BY = 3;
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

    }
}
