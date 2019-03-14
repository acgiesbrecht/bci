using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinBascula.Models
{
    public sealed class PuntoDeDescarga
    {
        public int Id { get; private set; }
        public string Descripcion { get; private set; }
        public Organizacion Organizacion { get; private set; }

        public override string ToString()
        {
            return Id + " - " + Descripcion;
        }

        public PuntoDeDescarga(int id, string descripcion, Organizacion organizacion)
        {
            Id = id;
            Descripcion = descripcion;
            Organizacion = organizacion;
        }            
    }    
}
