using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinBascula.Models
{
    public class XX_OPM_BCI_LOTE
    {
        [ExplicitKey]
        public string ID { get; set; }
        public string Year { get
            {
                return ID.Substring(0, 2);
            }
        }
        public string LoteCodigo { get
            {
                return ID.Substring(3, 3);
            }
        }

        public override string ToString()
        {
            return ID;
        }

    }
}
