using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinBascula.Models
{
    public sealed class Articulo
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public Organizacion Organizacion { get; set; }
        public TipoActividad TipoActividad { get; set; }
        public bool HasLote { get; set; }
    }
}
