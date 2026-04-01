using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public static class Seguridad
    {
        // método para saber si hay alguien logueado validando el objeto en sesión
        public static bool sesionActiva (object user)
        {
            Usuario usuario = user != null ? (Usuario)user : null; //la condición (user != null), si es verdadero lo transformamos, si es falso, estaba vacio y le asigno null a la variable


            if (usuario != null && usuario.Id !=0)
            {
                return true;
            }
            else
            {  
                return false;
            }
        }
        //método para saber si el usuario logueado tiene permisos de admin
        public static bool esAdmin(object user)
        {
            Usuario usuario = user != null ? (Usuario)user : null;

            if (usuario != null)
            {
                return usuario.Admin;
            }
            else
            {
                return false;
            }
        }

    }
}
