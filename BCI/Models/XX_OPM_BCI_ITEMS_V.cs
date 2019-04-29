using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Models
{
    public sealed class XX_OPM_BCI_ITEMS_V : INotifyPropertyChanged
    {
        [ExplicitKey]
        public long INVENTORY_ITEM_ID { get; set; }
        public string CODIGO_ITEM { get; set; }
        public string DESCRIPCION_ITEM { get; set; }
        public long ORGANIZATION_ID { get; set; }
        public long TIPO_ACTIVIDAD { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return DESCRIPCION_ITEM;
        }
    }
}
