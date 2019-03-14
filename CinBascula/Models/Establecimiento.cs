using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinBascula.Models
{
    public sealed class Establecimiento
    {
        public long Id { get; private set; }
        public string Descripcion { get; private set; }
        public string Ruc { get; private set; }

        public override string ToString()
        {
            return Descripcion + " - " + Ruc;
        }

        public Establecimiento(long id, string descripcion)
        {
            Id = id;
            Descripcion = Descripcion;
        }
    }
        
}
