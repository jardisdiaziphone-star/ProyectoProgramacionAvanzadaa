using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProyectoProgramacionAvanzada;

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class CalificacionesController : Controller
    {
        private CalificacionesDBEntities1 db = new CalificacionesDBEntities1();

        // Método auxiliar para validar sesión
        private bool UsuarioAutenticado()
        {
            return Session["Usuario"] != null;
        }

        // GET: Calificaciones
        public ActionResult Index()
        {
            if (!UsuarioAutenticado())
                return RedirectToAction("Index", "Login");

            var calificaciones = db.Calificaciones
                .Include(c => c.Estudiantes)
                .Include(c => c.Criterios);
            return View(calificaciones.ToList());
        }

        // GET: Calificaciones/Create
        public ActionResult Create()
        {
            if (!UsuarioAutenticado())
                return RedirectToAction("Index", "Login");

            ViewBag.IdEstudiante = new SelectList(db.Estudiantes, "IdEstudiante", "Nombre");
            ViewBag.IdCriterio = new SelectList(db.Criterios, "IdCriterio", "Nombre");
            return View();
        }

        // POST: Calificaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEstudiante,IdCriterio,NotaBase")] Calificaciones calificacion)
        {
            if (!UsuarioAutenticado())
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                var criterio = db.Criterios.Find(calificacion.IdCriterio);
                if (criterio != null)
                {
                    calificacion.NotaFinal = (calificacion.NotaBase / criterio.Porcentaje) * 100;
                }

                db.Calificaciones.Add(calificacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdEstudiante = new SelectList(db.Estudiantes, "IdEstudiante", "Nombre", calificacion.IdEstudiante);
            ViewBag.IdCriterio = new SelectList(db.Criterios, "IdCriterio", "Nombre", calificacion.IdCriterio);
            return View(calificacion);
        }

        // GET: Calificaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!UsuarioAutenticado())
                return RedirectToAction("Index", "Login");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Calificaciones calificacion = db.Calificaciones.Find(id);
            if (calificacion == null) return HttpNotFound();

            ViewBag.IdEstudiante = new SelectList(db.Estudiantes, "IdEstudiante", "Nombre", calificacion.IdEstudiante);
            ViewBag.IdCriterio = new SelectList(db.Criterios, "IdCriterio", "Nombre", calificacion.IdCriterio);
            return View(calificacion);
        }

        // POST: Calificaciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCalificacion,IdEstudiante,IdCriterio,NotaBase")] Calificaciones calificacion)
        {
            if (!UsuarioAutenticado())
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                var criterio = db.Criterios.Find(calificacion.IdCriterio);
                if (criterio != null)
                {
                    calificacion.NotaFinal = (calificacion.NotaBase / 100) * criterio.Porcentaje;
                }

                db.Entry(calificacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdEstudiante = new SelectList(db.Estudiantes, "IdEstudiante", "Nombre", calificacion.IdEstudiante);
            ViewBag.IdCriterio = new SelectList(db.Criterios, "IdCriterio", "Nombre", calificacion.IdCriterio);
            return View(calificacion);
        }

        // GET: Calificaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!UsuarioAutenticado())
                return RedirectToAction("Index", "Login");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Calificaciones calificacion = db.Calificaciones.Find(id);
            if (calificacion == null) return HttpNotFound();
            return View(calificacion);
        }

        // POST: Calificaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!UsuarioAutenticado())
                return RedirectToAction("Index", "Login");

            Calificaciones calificacion = db.Calificaciones.Find(id);
            db.Calificaciones.Remove(calificacion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Calificaciones/ExportPdf
        public ActionResult ExportPdf()
        {
            if (!UsuarioAutenticado())
                return RedirectToAction("Index", "Login");

            var calificaciones = db.Calificaciones
                .Include(c => c.Estudiantes)
                .Include(c => c.Criterios)
                .ToList();

            // Exporta Index, controlando si quieres fondo o no
            return new Rotativa.ViewAsPdf("Index", calificaciones)
            {
                FileName = "Calificaciones.pdf",
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--print-media-type --no-background" // usa --background si quieres fondo
            };
        }
    }
}

