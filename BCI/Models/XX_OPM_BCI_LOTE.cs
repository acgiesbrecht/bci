using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.Models
{
    public class XX_OPM_BCI_LOTE
    {
        
        [ExplicitKey]
        public string ID { get; set; }
        public string Descripcion
        {
            get
            {
                if (ID.Length == 11)
                {
                    return ID.Substring(3, 3);
                }
                else
                {
                    return ID;
                }                
            }
        }        

        public override string ToString()
        {
            return Descripcion;
        }
    }
}
