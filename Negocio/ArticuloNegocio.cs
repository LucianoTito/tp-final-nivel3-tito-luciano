using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Microsoft.Data.SqlClient;

namespace Negocio
{
    public class ArticuloNegocio
    {

        public List<Articulo> Listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.IdMarca, M.Descripcion AS Marca, A.IdCategoria, C.Descripcion AS Categoria, A.ImagenUrl, A.Precio FROM ARTICULOS A INNER JOIN MARCAS M ON A.IdMarca = M.Id INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id";
                datos.SetearConsulta(consulta);
                datos.EjecutarLectura();

                //Mapeo de la tabla sql a la clase Articulo
                while (datos.Lector.Read())
                {
                    lista.Add(MapearArticulo(datos)); //El método MapearArticulo se encarga de crear un nuevo objeto Articulo y asignarle los valores correspondientes a cada propiedad, utilizando los datos obtenidos del lector de la consulta SQL.
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

        //Trae UN solo artículo por su Id con una consulta parametrizada.
        public Articulo ObtenerPorId(int id)
        {
            Articulo articulo = null;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.IdMarca, M.Descripcion AS Marca, A.IdCategoria, C.Descripcion AS Categoria, A.ImagenUrl, A.Precio " +
                    "FROM ARTICULOS A INNER JOIN MARCAS M ON A.IdMarca = M.Id " +
                    "INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id " +
                    "WHERE A.Id = @id";

                datos.SetearConsulta(consulta);
                datos.SetearParametro("@id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    articulo = MapearArticulo(datos);
                }

                return articulo; 
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

        //El método Filtrar permite realizar búsquedas específicas en la base de datos según el campo, criterio y filtro proporcionados.
        public List<Articulo> Filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.IdMarca, M.Descripcion AS Marca, A.IdCategoria, C.Descripcion AS Categoria, A.ImagenUrl, A.Precio FROM ARTICULOS A INNER JOIN MARCAS M ON A.IdMarca = M.Id INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id WHERE ";

                if (campo == "Precio")
                {
                    // El precio debe ser numérico. Si no lo es, corto acá (evita romper la consulta y cualquier intento de inyección).
                    if (!decimal.TryParse(filtro, out decimal precio))
                        throw new ArgumentException("El valor del filtro de precio no es un número válido.");

                    // El operador sale de una whitelist, NO de lo que escriba el usuario.
                    string operador;
                    switch (criterio)
                    {
                        case "Mayor a": operador = ">"; break;
                        case "Menor a": operador = "<"; break;
                        default: operador = "="; break; 
                    }

                    consulta += "A.Precio " + operador + " @filtro";
                    datos.SetearParametro("@filtro", precio);
                }
                else
                {
                    // La COLUMNA sale de una whitelist. Si llega un campo desconocido, corto.
                    string columna;
                    switch (campo)
                    {
                        case "Nombre": columna = "A.Nombre"; break;
                        case "Marca": columna = "M.Descripcion"; break;
                        case "Categoría": columna = "C.Descripcion"; break;
                        case "Código": columna = "A.Codigo"; break;
                        default:
                            throw new ArgumentException("El campo de búsqueda no es válido.");
                    }

                    // El PATRÓN del LIKE se arma en C# y viaja como parámetro. El % no se concatena a la consulta cruda.
                    string patron;
                    switch (criterio)
                    {
                        case "Empieza con": patron = filtro + "%"; break;
                        case "Termina con": patron = "%" + filtro; break;
                        default: patron = "%" + filtro + "%"; break;
                    }

                    consulta += columna + " LIKE @filtro";
                    datos.SetearParametro("@filtro", patron);
                }

                datos.SetearConsulta(consulta);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(MapearArticulo(datos));
                }

                return lista;
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


        public void Eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("DELETE FROM ARTICULOS WHERE Id = @Id");
                datos.SetearParametro("@Id", id);
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

        public void AgregarArticulo(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) VALUES (@Codigo, @Nombre, @Descripcion, @IdMarca, @IdCategoria, @ImagenUrl, @Precio)");
                datos.SetearParametro("@Codigo", nuevo.Codigo);
                datos.SetearParametro("@Nombre", nuevo.Nombre);
                datos.SetearParametro("@Descripcion", nuevo.Descripcion);
                datos.SetearParametro("@IdMarca", nuevo.Marca.Id);
                datos.SetearParametro("@IdCategoria", nuevo.Categoria.Id);
                datos.SetearParametro("@ImagenUrl", nuevo.ImagenUrl);
                datos.SetearParametro("@Precio", nuevo.Precio);
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

        public void ModificarArticulo(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE ARTICULOS SET Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria, ImagenUrl = @ImagenUrl, Precio = @Precio WHERE Id = @Id");
                datos.SetearParametro("@Codigo", articulo.Codigo);
                datos.SetearParametro("@Nombre", articulo.Nombre);
                datos.SetearParametro("@Descripcion", articulo.Descripcion);
                datos.SetearParametro("@IdMarca", articulo.Marca.Id);
                datos.SetearParametro("@IdCategoria", articulo.Categoria.Id);
                datos.SetearParametro("@ImagenUrl", articulo.ImagenUrl);
                datos.SetearParametro("@Precio", articulo.Precio);
                datos.SetearParametro("@Id", articulo.Id);
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

        public Articulo MapearArticulo(AccesoDatos datos)
        {
            Articulo aux = new Articulo();

            if (!datos.Lector.IsDBNull(0)) aux.Id = datos.Lector.GetInt32(0);
            if (!datos.Lector.IsDBNull(1)) aux.Codigo = datos.Lector.GetString(1);
            if (!datos.Lector.IsDBNull(2)) aux.Nombre = datos.Lector.GetString(2);
            if (!datos.Lector.IsDBNull(3)) aux.Descripcion = datos.Lector.GetString(3);

            aux.Marca = new Marca();
            if (!datos.Lector.IsDBNull(4)) aux.Marca.Id = datos.Lector.GetInt32(4);
            if (!datos.Lector.IsDBNull(5)) aux.Marca.Descripcion = datos.Lector.GetString(5);

            aux.Categoria = new Categoria();
            if (!datos.Lector.IsDBNull(6)) aux.Categoria.Id = datos.Lector.GetInt32(6);
            if (!datos.Lector.IsDBNull(7)) aux.Categoria.Descripcion = datos.Lector.GetString(7);

            if (!datos.Lector.IsDBNull(8)) aux.ImagenUrl = datos.Lector.GetString(8);
            if (!datos.Lector.IsDBNull(9)) aux.Precio = datos.Lector.GetDecimal(9);

            return aux;
        }
    }
}
