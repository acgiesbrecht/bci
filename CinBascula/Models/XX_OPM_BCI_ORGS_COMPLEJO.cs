using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinBascula.Models
{
    public sealed class XX_OPM_BCI_ORGS_COMPLEJO : ReactiveObject
    {
        
        [Reactive] public long Id { get; set; }
        [Reactive] public string ShortDescription { get; set; }
        [Reactive] public string LongDerscription { get; set; }

    }    
}
