using System.Web.Mvc;

public class LoginController : Controller
{
    // GET: Login/Index
    public ActionResult Index()
    {
        return View(); // busca Views/Login/Index.cshtml
    }

    // POST: Login/Index
    [HttpPost]
    public ActionResult Index(string Username, string Password)
    {
        if (Username == "profesor" && Password == "1234")
        {
            // Guardamos sesión
            Session["Usuario"] = Username;
            return RedirectToAction("Index", "Calificaciones");
        }

        ViewBag.Error = "Usuario o contraseña incorrectos";
        return View();
    }

    // GET: Login/Logout
    public ActionResult Logout()
    {
        Session.Clear();
        return RedirectToAction("Index", "Login");
    }
}
