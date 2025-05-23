using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class CategoriaNegocio
    {
        public List<Categoria> Listar()
        {
			List<Categoria> lista = new List<Categoria>();
			AccesoDatos datos = new AccesoDatos();

			try
			{
				datos.setearConsulta("select id, descripcion from CATEGORIAS");
				datos.ejecutarLectura();
				while (datos.Lector.Read())
				{
					Categoria cat = new Categoria();
					cat.ID = (int) datos.Lector["id"];
					cat.Descripcion = (string) datos.Lector["Descripcion"];
                    lista.Add(cat);
                }
				return lista;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				datos.cerrarConexion();
			}
        }
		public void Agregar(Categoria NuevaCategoria)
		{
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("insert into CATEGORIAS (Descripcion) VALUES (@desc);");
                datos.setearParametros("@desc", NuevaCategoria.Descripcion);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
		public void Modificar(Categoria Cat)
		{
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update CATEGORIAS set Descripcion=@desc where ID=@id;");
                datos.setearParametros("@id", Cat.ID);
                datos.setearParametros("@desc", Cat.Descripcion);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void Eliminar(Categoria cat)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM CATEGORIAS WHERE Id=@Id");
                datos.setearParametros("@Id", cat.ID);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
