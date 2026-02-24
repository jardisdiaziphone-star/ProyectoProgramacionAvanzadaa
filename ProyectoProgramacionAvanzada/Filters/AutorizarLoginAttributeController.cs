using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoProgramacionAvanzada.Filters
{
    public class AutorizarLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var usuario = filterContext.HttpContext.Session["Usuario"];
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            // Permitimos el acceso libre al LoginController
            if (controller == "Login")
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            // Si no hay sesión, redirigimos al login
            if (usuario == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
            }

            base.OnActionExecuting(filterContext);
        }
    }

}