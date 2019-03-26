using Dapper.Contrib.Extensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinBascula.Models
{
    public sealed class XX_OPM_BCI_ITEMS_V : ReactiveObject
    {
        [ExplicitKey]
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
