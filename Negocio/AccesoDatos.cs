using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Negocio
{
    public class AccesoDatos
    {
        //Atributos privados
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;
        private SqlTransaction transaccion;

        public SqlDataReader Lector
        {
            get { return lector; }
        }

        //Constructor

        public AccesoDatos()
        {
            conexion = new SqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["conexionDB"].ConnectionString
            );
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

        //Métodos de transacción
        //Una transacción agrupa varias consultas: o se confirman TODAS (Commit) o no se aplica NINGUNA (Rollback).
        //Todos los EjecutarAccion/EjecutarLectura que vengan después corren dentro de la transacción.

        public void IniciarTransaccion()
        {
            comando.Connection = conexion;
            if (conexion.State == System.Data.ConnectionState.Closed)
            {
                conexion.Open();
            }

            transaccion = conexion.BeginTransaction();
            comando.Transaction = transaccion;
        }

        public void ConfirmarTransaccion()
        {
            transaccion.Commit();
            transaccion = null;
            comando.Transaction = null;
        }

        public void CancelarTransaccion()
        {
            //Si la conexión se cayó, SQL Server ya deshizo todo y la transacción queda sin conexión:
            //en ese caso no hay nada para deshacer (y Rollback tiraría otra excepción que taparía la original).
            if (transaccion != null && transaccion.Connection != null)
            {
                transaccion.Rollback();
            }
            transaccion = null;
            comando.Transaction = null;
        }

             public void CerrarConexion()
        {

            if (lector != null && !lector.IsClosed)
            {
                lector.Close();
            }


            //Si quedó una transacción abierta (no se confirmó ni se canceló), cerrar la conexión la deshace.
            if (conexion.State == System.Data.ConnectionState.Open)
            {
                conexion.Close();
            }
        }

    }
  
}
