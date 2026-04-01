using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class UsuarioNegocio
    {
        public bool Loguear(Usuario usuario)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT Id, email, pass, nombre, apellido, urlImagenPerfil, admin FROM USERS WHERE email = @email AND pass = @pass");

                datos.SetearParametro("@email", usuario.Email);
                datos.SetearParametro("@pass", usuario.Pass);

                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    usuario.Id = (int)datos.Lector["Id"];
                    usuario.Admin = (bool)datos.Lector["admin"];

                    if (!(datos.Lector["nombre"] is DBNull))
                    {
                        usuario.Nombre = (string)datos.Lector["nombre"];
                    }
                    if (!(datos.Lector["apellido"] is DBNull))
                    {
                        usuario.Apellido = (string)datos.Lector["apellido"];
                    }
                    if (!(datos.Lector["urlImagenPerfil"] is DBNull))
                    {
                        usuario.UrlImagenPerfil = (string)datos.Lector["urlImagenPerfil"];
                    }
                    return true;
                }

                return false;
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

        public void ActualizarPerfil(Usuario user)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE USERS SET nombre = @nombre, apellido = @apellido, urlImagenPerfil = @imagen WHERE Id = @id");

                datos.SetearParametro("@id", user.Id);
                datos.SetearParametro("@nombre", user.Nombre != null ? user.Nombre : (object)DBNull.Value);
                datos.SetearParametro("@apellido", user.Apellido != null ? user.Apellido : (object)DBNull.Value);
                datos.SetearParametro("@imagen", user.UrlImagenPerfil != null ? user.UrlImagenPerfil : (object)DBNull.Value);

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

        public int InsertarNuevo(Usuario nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("INSERT INTO USERS (email, pass, admin) OUTPUT inserted.Id VALUES (@email, @pass, 0)");

                datos.SetearParametro("@email", nuevo.Email);
                datos.SetearParametro("@pass", nuevo.Pass);
                //ejecuto la acción y capturo el ID que devuelve la base de datos
                return datos.EjecutarAccionScalar();
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