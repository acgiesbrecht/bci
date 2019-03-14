using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinBascula.Models
{
    public sealed class Organizacion
    {
        
        public string Id { get; private set; }
        public string Descripcion { get; private set; }

        public override string ToString()
        {
            return Id + " - " + Descripcion;
        }

        public Organizacion(string id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;
        }

        void createList()
        {
                      
        }                

    }
    
}
