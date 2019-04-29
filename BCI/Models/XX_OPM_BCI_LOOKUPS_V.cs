using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Models
{

    [Table("XX_OPM_BCI_LOOKUPS_V")]
    public class XX_OPM_BCI_LOOKUPS_V
    {        
        public string LOOKUP_TYPE { get; set; }        
        public string CODIGO { get; set; }
        public string SIGNIFICADO { get; set; }
        public string DESCRPCION { get; set; }
        public string TAG { get; set; }
    }
}
