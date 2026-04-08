using System.Web;
using System.Web.Mvc;
using ProyectoProgramacionAvanzada.Filters;

namespace ProyectoProgramacionAvanzada
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AutorizarLoginAttribute()); // aquí activas tu filtro global
        }
    }
}

