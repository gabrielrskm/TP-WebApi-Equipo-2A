using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace negocio
{
    public class AccesoDatos
    {
        string DB_CONFIG = ConfigurationManager.AppSettings["DBConectionData"];
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;
        public SqlDataReader Lector
        {
            get { return lector; }
        }
        public AccesoDatos()
        {
            string connectionString;
            if (!string.IsNullOrEmpty(DB_CONFIG))
            {
                connectionString = DB_CONFIG;
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            }
            conexion = new SqlConnection(connectionString);
            comando = new SqlCommand();
        }

        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al conectar a la base de datos", ex);
            }
        }
        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ejecutarAccionAndReturnId()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                object resultado = comando.ExecuteScalar();
                if (resultado != null)
                {
                    return Convert.ToInt32(resultado);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
        }
        public void setearParametros(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }
        public void cerrarConexion()
        {
            if (lector != null)
                lector.Close();
            conexion.Close();
        }
    }
}
