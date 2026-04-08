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
        // información: Contexto de base de datos generado por Entity Framework
        private CalificacionesDBEntities1 db = new CalificacionesDBEntities1();

        // información: Método auxiliar para validar sesión
        private bool UsuarioAutenticado()
        {
            return Session["Usuario"] != null;
        }

        
        // información: Muestra la lista de calificaciones registradas en el sistema
        public ActionResult Index()
        {
            if (!UsuarioAutenticado())
                return RedirectToAction("Index", "Login");

            // información: Incluye relaciones con Estudiantes y Criterios para mostrar datos completos
            var calificaciones = db.Calificaciones
                .Include(c => c.Estudiantes)
                .Include(c => c.Criterios);
            return View(calificaciones.ToList());
        }

        //  Details/5
        // información: Muestra los detalles de una calificación específica
        public ActionResult Details(int? id)
        {
            if (!UsuarioAutenticado())
                return RedirectToAction("Index", "Login");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Calificaciones calificacion = db.Calificaciones
                .Include(c => c.Estudiantes)
                .Include(c => c.Criterios)
                .FirstOrDefault(c => c.IdCalificacion == id);

            if (calificacion == null) return HttpNotFound();

            return View(calificacion);
        }

        // Create
        // información: Renderiza el formulario para crear una nueva calificación
        public ActionResult Create()
        {
            if (!UsuarioAutenticado())
                return RedirectToAction("Index", "Login");

            // información: Carga listas desplegables de estudiantes y criterios
            ViewBag.IdEstudiante = new SelectList(db.Estudiantes, "IdEstudiante", "Nombre");
            ViewBag.IdCriterio = new SelectList(db.Criterios, "IdCriterio", "Nombre");
            return View();
        }

        // POST: Calificaciones/Create
        // información: Procesa el formulario de creación y guarda la calificación en la BD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEstudiante,IdCriterio,NotaBase")] Calificaciones calificacion)
        {
            if (!UsuarioAutenticado())
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                // información: Calcula la nota final aplicando el porcentaje del criterio
                var criterio = db.Criterios.Find(calificacion.IdCriterio);
                if (criterio != null)
                {
                    calificacion.NotaFinal = (calificacion.NotaBase / criterio.Porcentaje) * 100;
                }

                db.Calificaciones.Add(calificacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // información: Si hay error, recarga las listas desplegables
            ViewBag.IdEstudiante = new SelectList(db.Estudiantes, "IdEstudiante", "Nombre", calificacion.IdEstudiante);
            ViewBag.IdCriterio = new SelectList(db.Criterios, "IdCriterio", "Nombre", calificacion.IdCriterio);
            return View(calificacion);
        }

        // Edit/5
        // información: Renderiza el formulario para editar una calificación existente
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
        // información: Procesa la edición y actualiza la calificación en la BD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCalificacion,IdEstudiante,IdCriterio,NotaBase")] Calificaciones calificacion)
        {
            if (!UsuarioAutenticado())
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                var original = db.Calificaciones.Find(calificacion.IdCalificacion);
                if (original == null) return HttpNotFound();

                // información: Actualiza campos editables
                original.IdEstudiante = calificacion.IdEstudiante;
                original.IdCriterio = calificacion.IdCriterio;
                original.NotaBase = calificacion.NotaBase;

                // información: Recalcula la nota final según el criterio
                var criterio = db.Criterios.Find(calificacion.IdCriterio);
                if (criterio != null)
                {
                    original.NotaFinal = (calificacion.NotaBase / criterio.Porcentaje) * 100;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdEstudiante = new SelectList(db.Estudiantes, "IdEstudiante", "Nombre", calificacion.IdEstudiante);
            ViewBag.IdCriterio = new SelectList(db.Criterios, "IdCriterio", "Nombre", calificacion.IdCriterio);
            return View(calificacion);
        }

        // Delete/5
        // información: Muestra confirmación antes de eliminar una calificación
        public ActionResult Delete(int? id)
        {
            if (!UsuarioAutenticado())
                return RedirectToAction("Index", "Login");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Calificaciones calificacion = db.Calificaciones.Find(id);
            if (calificacion == null) return HttpNotFound();
            return View(calificacion);
        }

        //Delete/5
        // información: Elimina definitivamente la calificación seleccionada
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
        // información: Exporta todas las calificaciones a un archivo PDF usando Rotativa
        public ActionResult ExportPdf()
        {
            if (!UsuarioAutenticado())
                return RedirectToAction("Index", "Login");

            var calificaciones = db.Calificaciones
                .Include(c => c.Estudiantes)
                .Include(c => c.Criterios)
                .ToList();

            return new Rotativa.ViewAsPdf("Index", calificaciones)
            {
                FileName = "Calificaciones.pdf",
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                CustomSwitches = "--print-media-type --no-background"
            };
        }
    }
}

