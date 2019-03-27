using Dapper.Contrib.Extensions;

namespace CinBascula.Models
{
    public class XX_OPM_BCI_CONTRATOS_V
    {        
        public long INVENTORY_ITEM_ID { get; set; }
        public string CODIGO_ITEM { get; set; }        
        public long ORGANIZATION_ID { get; set; }
        public string ORGANIZATION_CODE { get; set; }
        [ExplicitKey]
        public string NRO_CONTRATO { get; set; }                    
    }
}
