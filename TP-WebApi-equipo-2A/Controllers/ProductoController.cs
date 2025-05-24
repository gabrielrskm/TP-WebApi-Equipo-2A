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
        // GET api/values
        public IEnumerable<Articulo> Get()
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            List<Articulo> lista = articuloNegocio.Listar();
         
            return lista;
        }

        // GET api/values/5
        public Articulo Get(int id)
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            List<Articulo> lista = articuloNegocio.Listar();
            Articulo articulo = lista.Find(a => a.ID == id);
            return articulo;
        }

        // POST api/values
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
                Categoria busquedaCategoria = categorias.Find(e => productoDTO.IDCategoria == e.ID);
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
                articulo.Categoria = new Categoria { ID = productoDTO.IDCategoria};
                articulo.Codigo = productoDTO.Codigo;
                articulo.Precio = productoDTO.Precio;
                articulo.Marca = new Marca { ID = productoDTO.IDMarca };
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
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK, "El Producto fue creado correctamente.");
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
