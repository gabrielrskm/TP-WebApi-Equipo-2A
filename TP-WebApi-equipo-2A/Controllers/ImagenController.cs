using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using dominio;
using negocio;
using TP_WebApi_equipo_2A.Models;

namespace TP_WebApi_equipo_2A.Controllers
{
    public class ImagenController : ApiController
    {

        // POST: /api/Imagen/id
        public string Post(int id, [FromBody] ImagenDTO Imagenes)
        {
            if (Imagenes == null || Imagenes.Urls == null || Imagenes.Urls.Count == 0)
            {
                return "Se debe proporcionar una lista de URLs de imágenes.";
            }
            if (id == 0)
            {
                return "Se debe proporcionar el id del articulo";
            }
            var articulo = new ArticuloNegocio().FindById(id);
            if (articulo == null)
            {
                return "No se encontró el artículo con id " + id;
            }
            try
            {
                ImagenNegocio negocio = new ImagenNegocio();
                negocio.AgregarMasivoByArticuloId(id, Imagenes.Urls);
                return "Imágenes agregadas correctamente al artículo " + id;
            }
            catch (Exception ex)
            {
                return "Error al agregar imagenes: " + ex.Message;
            }
        }
    }
}
