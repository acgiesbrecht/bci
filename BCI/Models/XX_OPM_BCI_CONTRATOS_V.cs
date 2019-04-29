using Dapper.Contrib.Extensions;

namespace BCI.Models
{
    public class XX_OPM_BCI_CONTRATOS_V
    {
        [ExplicitKey]
        public string NRO_CONTRATO { get; set; }
        public long INVENTORY_ITEM_ID { get; set; }            
        public long ORGANIZATION_ID { get; set; }
        public string PROVEEDOR { get; set; }
        public string PESO_ORIGEN { get; set; }
    }
}
