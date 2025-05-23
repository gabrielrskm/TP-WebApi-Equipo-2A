using System.Web;
using System.Web.Mvc;

namespace TP_WebApi_equipo_2A
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
