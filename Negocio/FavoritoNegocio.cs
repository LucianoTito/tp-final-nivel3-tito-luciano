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
        public void InsertarFavorito(int idUsuario, int idArticulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("INSERT INTO FAVORITOS (IdUser, IdArticulo) VALUES (@idUser, @idArticulo)");
                datos.SetearParametro("@idUser", idUsuario);
                datos.SetearParametro("@idArticulo", idArticulo);
                datos.EjecutarAccion();
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
            catch (Exception ex)
            {
                throw ex;
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

                while(datos.Lector.Read())
                {

                    listaFavoritos.Add(articuloNegocio.MapearArticulo(datos));  

                }

                return listaFavoritos;
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

