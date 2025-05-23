using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> Listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("select a.id, a.Codigo, a.Nombre, a.Descripcion, a.Precio, a.IdMarca, m.Descripcion as marcaDescripcion, a.IdCategoria, c.Descripcion as categoriaDescripcion from ARTICULOS as a left join CATEGORIAS c on (c.id = a.IdCategoria) left join MARCAS m on (m.id=a.IdMarca)");    
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    lista.Add(InicializarObjeto(datos));
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
        public List<Articulo> Filtrar(string marca, string categoria)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("select a.id, a.Codigo, a.Nombre, a.Descripcion, a.Precio, a.IdMarca, m.Descripcion as marcaDescripcion, a.IdCategoria, c.Descripcion as categoriaDescripcion from ARTICULOS as a left join CATEGORIAS c on (c.id = a.IdCategoria) left join MARCAS m on (m.id=a.IdMarca) where c.descripcion like @categoria and m.descripcion like @marca");
                datos.setearParametros("@categoria", "%" + categoria + "%");
                datos.setearParametros("@marca", "%" + marca + "%");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    lista.Add(InicializarObjeto(datos));
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
        private Articulo InicializarObjeto(AccesoDatos datos)
        {
            int IdArticulo = (int)datos.Lector["id"];
            Articulo articulo = new Articulo();

            articulo.ID = IdArticulo;
            articulo.Codigo = (string)datos.Lector["Codigo"];
            articulo.Nombre = (string)datos.Lector["Nombre"];
            articulo.Descripcion = (string)datos.Lector["Descripcion"];
            articulo.Precio = (float)(decimal)datos.Lector["Precio"];
            if (!(datos.Lector["IdMarca"] is DBNull) && !(datos.Lector["marcaDescripcion"] is DBNull))
            {
                articulo.Marca = new Marca
                {
                    ID = (int)datos.Lector["IdMarca"],
                    Descripcion = (string)datos.Lector["marcaDescripcion"]
                };
            }
            if (!(datos.Lector["IdCategoria"] is DBNull) && !(datos.Lector["categoriaDescripcion"] is DBNull))
            {
                articulo.Categoria = new Categoria
                {
                    ID = (int)datos.Lector["IdCategoria"],
                    Descripcion = (string)datos.Lector["categoriaDescripcion"]
                };
            }
            //ToDo: Verificar que hacer en caso que no existan imagenes para este articulo
            articulo.Imagenes = new List<Imagen>();
            ImagenNegocio imgNegocio = new ImagenNegocio();
            articulo.Imagenes = imgNegocio.ListarByArticuloId(IdArticulo);
            return articulo;
        }
        public void Eliminar(Articulo articuloEliminar)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Eliminar todas las imagenes asociadas al articulo antes de eliminar el articulo
                ImagenNegocio imgNegocio = new ImagenNegocio();
                if(imgNegocio.EliminarByArticuloId(articuloEliminar.ID))
                {
                    //Eliminar articulo
                    datos.setearConsulta("DELETE FROM ARTICULOS WHERE Id=@Id");
                    datos.setearParametros("@Id", articuloEliminar.ID);
                    datos.ejecutarAccion();
                }
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
        public void Agregar(Articulo articuloNuevo)
        {

            AccesoDatos accesoDatos = new AccesoDatos();
            ImagenNegocio imgNegocio = new ImagenNegocio();
            try
            {
                string query = @"insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) 
                        values (@codigo, @nombre, @descripcion, @idMarca, @idCategoria, @precio);
                        select scope_identity();";

                accesoDatos.setearConsulta(query);
                accesoDatos.setearParametros("@codigo", articuloNuevo.Codigo);
                accesoDatos.setearParametros("@nombre", articuloNuevo.Nombre);
                accesoDatos.setearParametros("@descripcion", articuloNuevo.Descripcion);
                accesoDatos.setearParametros("@idMarca", articuloNuevo.Marca.ID);
                accesoDatos.setearParametros("@idCategoria", articuloNuevo.Categoria.ID);
                accesoDatos.setearParametros("@precio", articuloNuevo.Precio);
                int idArticulo = accesoDatos.ejecutarAccionAndReturnId();

                if (idArticulo != 0)
                {
                    List<string> urlsImagenes = articuloNuevo.Imagenes.Select(img => img.ImagenUrl).ToList();
                    imgNegocio.AgregarMasivoByArticuloId(idArticulo, urlsImagenes);
                }
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
        public void Modificar(Articulo articuloMod)
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            ImagenNegocio imgNegocio = new ImagenNegocio();
            try
            {
                string query = "update ARTICULOS set Codigo=@codigo, Nombre=@nombre, Descripcion=@descripcion, IdMarca=@idMarca, IdCategoria=@idCategoria, Precio=@precio where id=@id";
                accesoDatos.setearConsulta(query);
                accesoDatos.setearParametros("@codigo", articuloMod.Codigo);
                accesoDatos.setearParametros("@nombre", articuloMod.Nombre);
                accesoDatos.setearParametros("@descripcion", articuloMod.Descripcion);
                accesoDatos.setearParametros("@idMarca", articuloMod.Marca.ID);
                accesoDatos.setearParametros("@idCategoria", articuloMod.Categoria.ID);
                accesoDatos.setearParametros("@precio", articuloMod.Precio);
                accesoDatos.setearParametros("@id", articuloMod.ID);
                accesoDatos.ejecutarAccion();

                imgNegocio.UpdateByArticulo(articuloMod);
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
