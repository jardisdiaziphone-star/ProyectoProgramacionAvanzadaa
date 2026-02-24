using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoProgramacionAvanzada;

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class CriteriosController : Controller
    {
        private CalificacionesDBEntities1 db = new CalificacionesDBEntities1();



        // GET: Criterios
        public ActionResult Index()
        {
            var criterios = db.Criterios.Include(c => c.Profesores);
            return View(criterios.ToList());
        }

        // GET: Criterios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Criterios criterios = db.Criterios.Find(id);
            if (criterios == null)
            {
                return HttpNotFound();
            }
            return View(criterios);
        }

        // GET: Criterios/Create
        public ActionResult Create()
        {
            ViewBag.IdProfesor = new SelectList(db.Profesores, "IdProfesor", "Nombre");
            return View();
        }

        // POST: Criterios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCriterio,Nombre,Porcentaje,IdProfesor")] Criterios criterios)
        {
            if (ModelState.IsValid)
            {
                db.Criterios.Add(criterios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdProfesor = new SelectList(db.Profesores, "IdProfesor", "Nombre", criterios.IdProfesor);
            return View(criterios);
        }

        // GET: Criterios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Criterios criterios = db.Criterios.Find(id);
            if (criterios == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdProfesor = new SelectList(db.Profesores, "IdProfesor", "Nombre", criterios.IdProfesor);
            return View(criterios);
        }

        // POST: Criterios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCriterio,Nombre,Porcentaje,IdProfesor")] Criterios criterios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(criterios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdProfesor = new SelectList(db.Profesores, "IdProfesor", "Nombre", criterios.IdProfesor);
            return View(criterios);
        }

        // GET: Criterios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Criterios criterios = db.Criterios.Find(id);
            if (criterios == null)
            {
                return HttpNotFound();
            }
            return View(criterios);
        }

        // POST: Criterios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Criterios criterios = db.Criterios.Find(id);
            db.Criterios.Remove(criterios);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
