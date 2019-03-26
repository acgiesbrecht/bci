using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinBascula.Models
{
    
    public class XX_OPM_BCI_LOOKUPS_V : ReactiveObject
    {        
        [Reactive] public string LOOKUP_TYPE { get; set; }
        [Key]
        [Reactive] public string CODIGO { get; set; }
        [Reactive] public string SIGNIFICADO { get; set; }
        [Reactive] public string DESCRPCION { get; set; }
        [Reactive] public string TAG { get; set; }
    }
}
