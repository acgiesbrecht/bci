using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinBascula.Models
{
    [Table("APPS.XX_OPM_BCI_ITEMS_V")]
    public sealed class XX_OPM_BCI_ITEMS_V : ReactiveObject
    {
        [Key]
        [Reactive] public long INVENTORY_ITEM_ID { get; set; }
        [Reactive] public string CODIGO_ITEM { get; set; }
        [Reactive] public string DESCRIPCION_ITEM { get; set; }
        [Reactive] public long ORGANIZATION_ID { get; set; }
        [Reactive] public string TIPO_ACTIVIDAD { get; set; }        

        public override string ToString()
        {
            return DESCRIPCION_ITEM;
        }
    }
}
