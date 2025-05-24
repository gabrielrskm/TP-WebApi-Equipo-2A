using dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TP_WebApi_equipo_2A.Models
{
    public class ProductoDTO
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? IDMarca { get; set; }
        public int? IDCategoria { get; set; }
        public List<string> Imagenes { get; set; }
        public float? Precio { get; set; }
    }
}