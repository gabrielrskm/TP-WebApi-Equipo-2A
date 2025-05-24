using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TP_WebApi_equipo_2A.Models;

namespace TP_WebApi_equipo_2A.Controllers
{
    public class ProductoController : ApiController
    {
        // GET api/Producto
        public IEnumerable<Articulo> Get()
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            List<Articulo> lista = articuloNegocio.Listar();

            return lista;
        }
        // GET api/Producto/{id}
        public Articulo Get(int id)
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            List<Articulo> lista = articuloNegocio.Listar();
            Articulo articulo = lista.Find(a => a.ID == id);
            return articulo;
        }
        // POST api/Producto
        public HttpResponseMessage Post([FromBody] ProductoDTO productoDTO)
        {
            bool validation = (productoDTO == null) ||
                productoDTO.IDMarca < 1 ||
                productoDTO.Imagenes == null ||
                productoDTO.Codigo == null ||
                productoDTO.Descripcion == string.Empty ||
                productoDTO.Precio < 0 ||
                productoDTO.IDCategoria < 1;

            if (validation)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Producto con items incompletos");
            }

            try
            {
                Articulo articulo = new Articulo();
                CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
                List<Categoria> categorias = categoriaNegocio.Listar();
                Categoria busquedaCategoria = categorias.Find(e => (int)productoDTO.IDCategoria == e.ID);
                if (busquedaCategoria == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "IDCategoria no existe");
                }
                MarcaNegocio marcaNegocio = new MarcaNegocio();
                List<Marca> marcas = marcaNegocio.Listar();
                Marca busquedaMarca = marcas.Find(value => productoDTO.IDMarca == value.ID);
                if (busquedaMarca == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "IDMarca no existe");
                }

                articulo.Descripcion = productoDTO.Descripcion;
                articulo.Categoria = new Categoria { ID = (int)productoDTO.IDCategoria };
                articulo.Codigo = (string)productoDTO.Codigo;
                articulo.Precio = (int)productoDTO.Precio;
                articulo.Marca = new Marca { ID = (int)productoDTO.IDMarca };
                articulo.Nombre = productoDTO.Nombre;

                List<Imagen> listaImagen = new List<Imagen>();
                foreach (var item in productoDTO.Imagenes)
                {
                    Imagen imagen = new Imagen { ImagenUrl = item };
                    listaImagen.Add(imagen);
                }
                articulo.Imagenes = listaImagen;
                ArticuloNegocio articuloNegocio = new ArticuloNegocio();
                articuloNegocio.Agregar(articulo);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK, "El Producto fue creado correctamente.");
        }

        // PATCH api/Producto/{id}
        public HttpResponseMessage Patch(int id, [FromBody] ProductoDTO productoDTO)
        {
            if (productoDTO == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Datos inválidos");
            try
            {
                ArticuloNegocio artNegocio = new ArticuloNegocio();
                Articulo articulo = new Articulo();
                articulo = artNegocio.FindById(id);

                if (productoDTO.Codigo != null )
                {
                    articulo.Codigo = productoDTO.Codigo;
                }
                if (productoDTO.Nombre != null)
                {
                    articulo.Nombre = productoDTO.Nombre;
                }
                if (productoDTO.Descripcion != null)
                {
                    articulo.Descripcion = productoDTO.Descripcion;
                }
                if (productoDTO.IDMarca != null)
                {
                    MarcaNegocio marcaNegocio = new MarcaNegocio();
                    List<Marca> marcas = marcaNegocio.Listar();
                    Marca busquedaMarca = marcas.Find(value => productoDTO.IDMarca == value.ID);
                    if (busquedaMarca == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "IDMarca no existe");
                    }
                    articulo.Marca = new Marca { ID = (int)productoDTO.IDMarca };
                }
                if (productoDTO.IDCategoria != null)
                {
                    CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
                    List<Categoria> categorias = categoriaNegocio.Listar();
                    Categoria busquedaCategoria = categorias.Find(e => productoDTO.IDCategoria == e.ID);
                    if (busquedaCategoria == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "IDCategoria no existe");
                    }
                    articulo.Categoria = new Categoria { ID = (int)productoDTO.IDCategoria };
                }
                if (productoDTO.Imagenes != null)
                {
                    List<Imagen> listaImagen = new List<Imagen>();
                    foreach (var item in productoDTO.Imagenes)
                    {
                        Imagen imagen = new Imagen { ImagenUrl = item };
                        listaImagen.Add(imagen);
                    }
                    articulo.Imagenes = listaImagen;
                }
                if (productoDTO.Precio != null)
                {
                    articulo.Precio = (float)productoDTO.Precio;
                }
                artNegocio.Modificar(articulo);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK, "El Producto fue Modificado exitosamente correctamente.");
        }
        // DELETE api/Producto/{id}
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                Articulo articulo = new Articulo();
                ArticuloNegocio articuloNegocio = new ArticuloNegocio();
                articulo = articuloNegocio.FindById(id);
                if (articulo != null)
                {
                    articuloNegocio.Eliminar(articulo);
                    return Request.CreateResponse(HttpStatusCode.OK, "Eliminado ok.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "El articulo a eliminar no existe.");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
