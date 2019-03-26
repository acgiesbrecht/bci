using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinBascula.Models
{
    class OracleDbContext : DbContext
    {
        
        //public virtual DbSet<XX_OPM_BCI_CONTRATOS_V> XX_OPM_BCI_CONTRATOS_V { get; set; }
        //public virtual DbSet<XX_OPM_BCI_ESTAB> XX_OPM_BCI_ESTAB { get; set; }
        public virtual DbSet<XX_OPM_BCI_ITEMS_V> XX_OPM_BCI_ITEMS_V { get; set; }
        //public virtual DbSet<XX_OPM_BCI_LOOKUPS_V> XX_OPM_BCI_LOOKUPS_V { get; set; }
        //public virtual DbSet<XX_OPM_BCI_ORGS_COMPLEJO> XX_OPM_BCI_ORGS_COMPLEJO { get; set; }
        //public virtual DbSet<XX_OPM_BCI_PESADAS_ALL> XX_OPM_BCI_PESADAS_ALL { get; set; }
        //public virtual DbSet<XX_OPM_BCI_PUNTO_DESCARGA> XX_OPM_BCI_PUNTO_DESCARGA { get; set; }
        //public virtual DbSet<XX_OPM_BCI_TIPO_ACTIVIDAD> XX_OPM_BCI_TIPO_ACTIVIDAD { get; set; }

    }
}
