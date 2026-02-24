using System.Linq;
using System.Web.Mvc;
using ProyectoProgramacionAvanzada;

namespace ProgramacionAvanzadaP.Controllers
{
    public class EstudiantesController : Controller
    {
        private CalificacionesDBEntities db = new CalificacionesDBEntities();

        // GET: Estudiantes
        public ActionResult Index()
        {
            var estudiantes = db.Estudiantes.ToList();
            return View(estudiantes);
        }

        // GET: Estudiantes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Estudiantes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Estudiantes estudiante)
        {
            if (ModelState.IsValid)
            {
                db.Estudiantes.Add(estudiante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estudiante);
        }
    }
}

