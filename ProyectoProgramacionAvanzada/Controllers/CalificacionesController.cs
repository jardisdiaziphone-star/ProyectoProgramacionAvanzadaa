using System.Linq;
using System.Web.Mvc;
using ProyectoProgramacionAvanzada; 

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class CalificacionesController : Controller
    {
        private CalificacionesDBEntities db = new CalificacionesDBEntities();

        // GET: Calificaciones
        public ActionResult Index()
        {
            var calificaciones = db.Calificaciones.Include("Estudiantes").Include("Criterios").ToList();
            return View(calificaciones);
        }

        // GET: Calificaciones/Create
        public ActionResult Create()
        {
            ViewBag.IdEstudiante = new SelectList(db.Estudiantes, "IdEstudiante", "Nombre");
            ViewBag.IdCriterio = new SelectList(db.Criterios, "IdCriterio", "Nombre");
            return View();
        }

        // POST: Calificaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Calificaciones calificacion)
        {
            if (ModelState.IsValid)
            {
                var criterio = db.Criterios.Find(calificacion.IdCriterio);
                calificacion.NotaFinal = calificacion.NotaBase * (criterio.Porcentaje / 100);

                db.Calificaciones.Add(calificacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdEstudiante = new SelectList(db.Estudiantes, "IdEstudiante", "Nombre", calificacion.IdEstudiante);
            ViewBag.IdCriterio = new SelectList(db.Criterios, "IdCriterio", "Nombre", calificacion.IdCriterio);
            return View(calificacion);
        }
    }
}