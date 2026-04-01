using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;


namespace Negocio
{
    public class AccesoDatos
    {
        //Atributos privados
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector
        {
            get { return lector; }
        }

        //Constructor

        public AccesoDatos()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=CATALOGO_WEB_DB; integrated security=true; TrustServerCertificate=True");
            comando = new SqlCommand();
        }
        
        //Métodos de configuración
        public void SetearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void SetearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);

        }

        //Métodos de ejecución

        public void EjecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                if (conexion.State == System.Data.ConnectionState.Closed)
                {
                    conexion.Open();
                }

                lector = comando.ExecuteReader();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void EjecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                if (conexion.State == System.Data.ConnectionState.Closed)
                {
                    conexion.Open();
                }
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int EjecutarAccionScalar()
        {
            try
            {
                comando.Connection = conexion;
                conexion.Open();

                return (int)comando.ExecuteScalar();


            }
            catch (Exception)
            {

                throw;
            }
        }

             public void CerrarConexion()
        {
           
            if (lector != null && !lector.IsClosed)
            {
                lector.Close();
            }

          
            if (conexion.State == System.Data.ConnectionState.Open)
            {
                conexion.Close();
            }
        }

    }
  
}
