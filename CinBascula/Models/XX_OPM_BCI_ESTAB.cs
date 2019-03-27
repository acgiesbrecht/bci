using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinBascula.Models
{

    public class XX_OPM_BCI_ESTAB
    {

        [ExplicitKey]
        public string Id { get; set; }
        public string Significado { get; set; }
        public string Derscripcion { get; set; }
        public string ApAr { get; set; }

        public override string ToString()
        {
            return Significado;
        }

    }
        
}
