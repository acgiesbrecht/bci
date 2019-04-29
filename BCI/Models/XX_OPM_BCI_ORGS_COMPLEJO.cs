using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Models
{
    public sealed class XX_OPM_BCI_ORGS_COMPLEJO
    {

        [ExplicitKey]
        public long Id { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }

        public override string ToString()
        {
            return Description + " - " + Tag;
        }

    }    
}
