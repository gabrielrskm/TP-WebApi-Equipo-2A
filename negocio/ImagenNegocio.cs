using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;
using Microsoft.SqlServer.Server;

namespace negocio
{
    public class ImagenNegocio
    {
        public List<Imagen> ListarByArticuloId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            List<Imagen> lista = new List<Imagen>();
            try
            {
                datos.setearConsulta("select Id, ImagenUrl from IMAGENES where IdArticulo = @IdArticulo");
                datos.setearParametros("@IdArticulo", id);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Imagen imagen = new Imagen
                    {
                        ID = (int)datos.Lector["Id"],
                        ImagenUrl = (string)datos.Lector["ImagenUrl"]
                    };
                    lista.Add(imagen);
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
        public bool EliminarByArticuloId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM IMAGENES WHERE IdArticulo=@Id");
                datos.setearParametros("@Id", id);
                datos.ejecutarAccion();
                return true;
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
        public void UpdateByArticulo(Articulo articulo)
        {
            List<Imagen> imagenesPreCargadas = ListarByArticuloId(articulo.ID);
            List<string> idsImagenesToAdd = new List<string>();
            List<int> idsImagenesToDelete = new List<int>();

            //Si la imagen no existia en la bd, agregarla
            foreach (Imagen img in articulo.Imagenes)
            {
                bool agregar = true;
                foreach (var pre in imagenesPreCargadas)
                {
                    if (img.ID == pre.ID)
                    {
                        agregar = false;
                        break;
                    }
                }

                if (agregar)
                {
                    idsImagenesToAdd.Add(img.ImagenUrl);
                }
            }

            //Si la imagen existe en la bd pero no existe en articuloMod, eliminarla de la bd
            foreach (Imagen pre in imagenesPreCargadas)
            {
                bool eliminar = true;
                foreach (Imagen img in articulo.Imagenes)
                {
                    if (pre.ID == img.ID)
                    {
                        eliminar = false;
                        break;
                    }
                }

                if (eliminar)
                {
                    idsImagenesToDelete.Add(pre.ID);
                }
            }
            if (idsImagenesToAdd.Count > 0) AgregarMasivoByArticuloId(articulo.ID, idsImagenesToAdd);
            if (idsImagenesToDelete.Count > 0) EliminarMasivoByIds(idsImagenesToDelete);
        }
        public void AgregarMasivoByArticuloId(int id, List<string> urlImagenes)
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                string query = "insert into imagenes (IdArticulo,ImagenUrl) values ";
                foreach (string url in urlImagenes)
                {
                    query += "(" + id + ",'" + url + "'),";
                }
                query = query.Substring(0, query.Length - 1);
                Console.WriteLine(query);
                accesoDatos.setearConsulta(query);
                accesoDatos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }
        public void EliminarMasivoByIds(List<int> ids)
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                string query = "DELETE FROM IMAGENES where id in (";
                foreach (int id in ids)
                {
                    query += id + ",";
                }
                query = query.Substring(0, query.Length - 1);
                query += ")";
                Console.WriteLine(query);
                accesoDatos.setearConsulta(query);
                accesoDatos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }
    }
}
