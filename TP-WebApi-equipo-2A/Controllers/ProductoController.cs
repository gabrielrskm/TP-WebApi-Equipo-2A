using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public void Post([FromBody] ProductoDTO value)
        {
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
