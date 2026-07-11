using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class FavoritoNegocio
    {
        //Chequea si un artículo ya está en la lista de favoritos de un usuario.
        public bool Existe(int idUsuario, int idArticulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT Id FROM FAVORITOS WHERE IdUser = @idUser AND IdArticulo = @idArticulo");
                datos.SetearParametro("@idUser", idUsuario);
                datos.SetearParametro("@idArticulo", idArticulo);
                datos.EjecutarLectura();

                return datos.Lector.Read(); // true si ya existe la fila
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void InsertarFavorito(int idUsuario, int idArticulo)
        {
            //Evito duplicados por código: si ya está en favoritos, no vuelvo a insertar.
            //(No podemos usar una restricción UNIQUE en la DB porque no se puede modificar el esquema.)
            if (Existe(idUsuario, idArticulo))
                return;

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("INSERT INTO FAVORITOS (IdUser, IdArticulo) VALUES (@idUser, @idArticulo)");
                datos.SetearParametro("@idUser", idUsuario);
                datos.SetearParametro("@idArticulo", idArticulo);
                datos.EjecutarAccion();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void EliminarFavorito(int idUsuario, int idArticulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("DELETE FROM FAVORITOS WHERE IdUser = @idUser AND IdArticulo = @idArticulo");
                datos.SetearParametro("@idUser", idUsuario);
                datos.SetearParametro("@idArticulo", idArticulo);
                datos.EjecutarAccion();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public List<Articulo> ListarFavoritos(int idUsuario)
        {
            List<Articulo> listaFavoritos = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            ArticuloNegocio articuloNegocio = new ArticuloNegocio();

            try
            {
                string consulta = "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.IdMarca, M.Descripcion AS Marca, A.IdCategoria, C.Descripcion " +
                    "AS Categoria, A.ImagenUrl, A.Precio " +
                    "FROM ARTICULOS A INNER JOIN MARCAS M ON A.IdMarca = M.Id " +
                    "INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id " +
                    "INNER JOIN FAVORITOS F ON F.IdArticulo = A.Id " +
                    "WHERE F.IdUser = @idUser";

                datos.SetearConsulta(consulta);
                datos.SetearParametro("@idUser", idUsuario);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {

                    listaFavoritos.Add(articuloNegocio.MapearArticulo(datos));

                }

                return listaFavoritos;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.CerrarConexion();
            }


        }
    }
}
