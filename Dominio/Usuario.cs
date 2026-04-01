using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string UrlImagenPerfil { get; set; }


        //Propiedad p/ saber si es usuario normal o admin (1=Normal, 2=Admin)
        public bool Admin { get; set; }
    }

}
