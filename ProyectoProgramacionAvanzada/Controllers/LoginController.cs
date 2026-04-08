using System.Web.Mvc;

public class LoginController : Controller
{
    
    // información: Renderiza la vista de inicio de sesión (formulario)
    public ActionResult Index()
    {
        return View(); // busca Views/Login/Index.cshtml
    }

    // información: Procesa el formulario de login, valida usuario y contraseña
    [HttpPost]
    public ActionResult Index(string Username, string Password)
    {
        // información: Validación simple con credenciales fijas
        if (Username == "profesor" && Password == "1234")
        {
            // información: Guardamos el usuario en sesión para controlar acceso
            Session["Usuario"] = Username;

            // información: Redirige al controlador Calificaciones si login es correcto
            return RedirectToAction("Index", "Calificaciones");
        }

        // información: Si las credenciales no son válidas, muestra error en la vista
        ViewBag.Error = "Usuario o contraseña incorrectos";
        return View();
    }


    // información: Cierra sesión eliminando datos de Session y regresa al login
    public ActionResult Logout()
    {
        Session.Clear(); // información: Limpia toda la sesión
        return RedirectToAction("Index", "Login"); // información: Redirige al formulario de login
    }
}

