﻿using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinBascula.Models
{
    public sealed class XX_OPM_BCI_ITEMS_V
    {
        [ExplicitKey]
        public long INVENTORY_ITEM_ID { get; set; }
        public string CODIGO_ITEM { get; set; }
        public string DESCRIPCION_ITEM { get; set; }
        public long ORGANIZATION_ID { get; set; }
        public string TIPO_ACTIVIDAD { get; set; }        

        public override string ToString()
        {
            return DESCRIPCION_ITEM;
        }
    }
}