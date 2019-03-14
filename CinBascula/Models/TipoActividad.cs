using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinBascula.Models
{
    public sealed class TipoActividad
    {

        public long Id { get; private set; }
        public string Descripcion { get; private set; }
        public string TipoEstablecimiento { get; private set; }

        public override string ToString()
        {
            return Descripcion;
        }

        public TipoActividad(long id, string descripcion, string tipoEstablecimiento)
        {
            Id = id;
            Descripcion = descripcion;
            TipoEstablecimiento = tipoEstablecimiento;
        }        

    }
    
}
