using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class CategoriaNegocio
    {
        public List<Categoria> listar()
        {
            List<Categoria> lista = new List<Categoria>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT Id, Descripcion FROM CATEGORIAS");
                datos.EjecutarLectura();


                while (datos.Lector.Read())
                {
                    Categoria aux = new Categoria();
                    if (!datos.Lector.IsDBNull(0)) aux.Id = datos.Lector.GetInt32(0);
                    if (!datos.Lector.IsDBNull(1)) aux.Descripcion = datos.Lector.GetString(1);
                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

    }
}
