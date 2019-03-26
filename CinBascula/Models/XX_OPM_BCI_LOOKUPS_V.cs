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

    [Table("XX_OPM_BCI_LOOKUPS_V")]
    public class XX_OPM_BCI_LOOKUPS_V : ReactiveObject
    {        
        [Reactive] public string LOOKUP_TYPE { get; set; }        
        [Reactive] public string CODIGO { get; set; }
        [Reactive] public string SIGNIFICADO { get; set; }
        [Reactive] public string DESCRPCION { get; set; }
        [Reactive] public string TAG { get; set; }
    }
}
