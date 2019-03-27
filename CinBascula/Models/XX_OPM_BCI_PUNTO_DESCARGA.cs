using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinBascula.Models
{
    public sealed class XX_OPM_BCI_PUNTO_DESCARGA
    {

        [ExplicitKey]
        public string Id { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }

        public override string ToString()
        {
            return Description;
        }

    }    
}
