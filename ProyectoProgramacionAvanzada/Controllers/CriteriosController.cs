using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProyectoProgramacionAvanzada;
using System.Web.Mvc;


namespace ProyectoProgramacionAvanzada.Controllers
{
    public class CriteriosController : Controller
    {
        private CalificacionesDBEntities db = new CalificacionesDBEntities();

        // GET: Criterios
        public ActionResult Index()
        {
            var criterios = db.Criterios.ToList();
            return View(criterios);
        }

        // GET: Criterios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Criterios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Criterios criterio)
        {
            if (ModelState.IsValid)
            {
                db.Criterios.Add(criterio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(criterio);
        }
    }
}

