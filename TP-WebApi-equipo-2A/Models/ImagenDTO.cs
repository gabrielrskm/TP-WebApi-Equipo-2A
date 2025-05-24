using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP_WebApi_equipo_2A.Models
{
    public class ImagenDTO
    {
        public int IdArticulo { get; set; }
        public List<string> Urls { get; set; }
    }
}