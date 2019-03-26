using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CinBascula.Models
{
    public class XX_OPM_BCI_CONTRATOS_V : ReactiveObject
    {        
        [Reactive] public long INVENTORY_ITEM_ID { get; set; }
        [Reactive] public string CODIGO_ITEM { get; set; }        
        [Reactive] public long ORGANIZATION_ID { get; set; }
        [Reactive] public string ORGANIZATION_CODE { get; set; }
        [Key]
        [Reactive] public string NRO_CONTRATO { get; set; }                    
    }
}
