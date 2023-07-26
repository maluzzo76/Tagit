using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LabelUp_Admin.Models;

namespace LabelUp_Admin.Controllers
{
    public class tbl_Estampa_GrupoEstampaController : CustomController
    {
        public BusinessEntities db = new BusinessEntities();

        // GET: tbl_Estampa_GrupoEstampa
        public ActionResult Index()
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            var tbl_Estampa_GrupoEstampa = db.tbl_Estampa_GrupoEstampa.Include(t => t.tbl_Estampas).Include(t => t.tbl_Grupo_Estampas);
            return View(tbl_Estampa_GrupoEstampa.ToList());
        }

        // GET: tbl_Estampa_GrupoEstampa/Details/5
        public ActionResult Details(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Estampa_GrupoEstampa tbl_Estampa_GrupoEstampa = db.tbl_Estampa_GrupoEstampa.Find(id);
            if (tbl_Estampa_GrupoEstampa == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Estampa_GrupoEstampa);
        }

        // GET: tbl_Estampa_GrupoEstampa/Create
        public ActionResult Create()
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            ViewBag.Id_Estampa = new SelectList(db.tbl_Estampas, "Id", "Codigo");
            ViewBag.Id_GrupoEstampa = new SelectList(db.tbl_Grupo_Estampas, "Id", "Codigo");
            return View();
        }

        // POST: tbl_Estampa_GrupoEstampa/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_GrupoEstampa,Id_Estampa,Activo,Eliminado")] tbl_Estampa_GrupoEstampa tbl_Estampa_GrupoEstampa)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (ModelState.IsValid)
            {
                db.tbl_Estampa_GrupoEstampa.Add(tbl_Estampa_GrupoEstampa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Estampa = new SelectList(db.tbl_Estampas, "Id", "Codigo", tbl_Estampa_GrupoEstampa.Id_Estampa);
            ViewBag.Id_GrupoEstampa = new SelectList(db.tbl_Grupo_Estampas, "Id", "Codigo", tbl_Estampa_GrupoEstampa.Id_GrupoEstampa);
            return View(tbl_Estampa_GrupoEstampa);
        }

        // GET: tbl_Estampa_GrupoEstampa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Estampa_GrupoEstampa tbl_Estampa_GrupoEstampa = db.tbl_Estampa_GrupoEstampa.Find(id);
            if (tbl_Estampa_GrupoEstampa == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Estampa = new SelectList(db.tbl_Estampas, "Id", "Codigo", tbl_Estampa_GrupoEstampa.Id_Estampa);
            ViewBag.Id_GrupoEstampa = new SelectList(db.tbl_Grupo_Estampas, "Id", "Codigo", tbl_Estampa_GrupoEstampa.Id_GrupoEstampa);
            return View(tbl_Estampa_GrupoEstampa);
        }

        // POST: tbl_Estampa_GrupoEstampa/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_GrupoEstampa,Id_Estampa,Activo,Eliminado")] tbl_Estampa_GrupoEstampa tbl_Estampa_GrupoEstampa)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (ModelState.IsValid)
            {
                db.Entry(tbl_Estampa_GrupoEstampa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Estampa = new SelectList(db.tbl_Estampas, "Id", "Codigo", tbl_Estampa_GrupoEstampa.Id_Estampa);
            ViewBag.Id_GrupoEstampa = new SelectList(db.tbl_Grupo_Estampas, "Id", "Codigo", tbl_Estampa_GrupoEstampa.Id_GrupoEstampa);
            return View(tbl_Estampa_GrupoEstampa);
        }

        // GET: tbl_Estampa_GrupoEstampa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Estampa_GrupoEstampa tbl_Estampa_GrupoEstampa = db.tbl_Estampa_GrupoEstampa.Find(id);
            if (tbl_Estampa_GrupoEstampa == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Estampa_GrupoEstampa);
        }

        // POST: tbl_Estampa_GrupoEstampa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            tbl_Estampa_GrupoEstampa tbl_Estampa_GrupoEstampa = db.tbl_Estampa_GrupoEstampa.Find(id);
            db.tbl_Estampa_GrupoEstampa.Remove(tbl_Estampa_GrupoEstampa);
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
