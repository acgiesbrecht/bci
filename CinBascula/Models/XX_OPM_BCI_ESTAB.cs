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

    public class XX_OPM_BCI_ESTAB : ReactiveObject
    {

        [ExplicitKey]
        [Reactive] public string Id { get; set; }
        [Reactive] public string Significado { get; set; }
        [Reactive] public string Derscripcion { get; set; }
        [Reactive] public string ApAr { get; set; }

        public override string ToString()
        {
            return Significado;
        }

    }
        
}
