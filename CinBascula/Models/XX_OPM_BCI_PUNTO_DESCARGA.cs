using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinBascula.Models
{
    public sealed class XX_OPM_BCI_PUNTO_DESCARGA : ReactiveObject
    {

        [Reactive] public string Id { get; set; }
        [Reactive] public string Description { get; set; }
        [Reactive] public string OrganisationTag { get; set; }

    }    
}
