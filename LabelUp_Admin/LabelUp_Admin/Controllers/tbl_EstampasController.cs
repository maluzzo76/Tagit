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
    public class tbl_EstampasController : CustomController
    {

        // GET: tbl_Estampas
        public ActionResult Index(string SearchString, int pagina=1)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            var _r = from _o in db.tbl_Estampas.Include(t => t.tbl_Imagenes)
                     where _o.Eliminado == false
                     orderby _o.Id descending
                     select _o;


            if (!string.IsNullOrEmpty(SearchString))
            {
                _r = from _o in db.tbl_Estampas.Include(t => t.tbl_Imagenes)
                     where _o.Nombre.Contains(SearchString) || _o.Codigo.Contains(SearchString) && _o.Eliminado == false
                     orderby _o.Id descending
                     select _o;
            }

            Pagination<tbl_Estampas> _page = new Pagination<tbl_Estampas>();
            return View(_page.paginado(_r, pagina));
        }


        // GET: tbl_Estampas/Details/5
        public ActionResult Details(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Estampas tbl_Estampas = db.tbl_Estampas.Find(id);
            if (tbl_Estampas == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Estampas);
        }

        // GET: tbl_Estampas/Create
        public ActionResult Create()
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            ViewBag.Id_Imagen = new SelectList(db.tbl_Imagenes.Where(e => e.tbl_grupoImagen.codigo == "1").ToList<tbl_Imagenes>(), "Id", "URI");
            return View();
        }

        // POST: tbl_Estampas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,Nombre,Descripcion,Id_Imagen,Visible,Eliminado")] tbl_Estampas tbl_Estampas)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (ModelState.IsValid)
            {
                tbl_Estampas.Visible = (tbl_Estampas.Visible == null)? false: tbl_Estampas.Visible;
                tbl_Estampas.Id_Imagen = (tbl_Estampas.Id_Imagen == null) ? 1 : tbl_Estampas.Id_Imagen;
                tbl_Estampas.Eliminado = false;
                db.tbl_Estampas.Add(tbl_Estampas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Imagen = new SelectList(db.tbl_Imagenes.Where(e => e.tbl_grupoImagen.codigo == "1").ToList<tbl_Imagenes>(), "Id", "Nombre", tbl_Estampas.Id_Imagen);
            return View(tbl_Estampas);
        }

        // GET: tbl_Estampas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            tbl_Estampas tbl_Estampas = db.tbl_Estampas.Find(id);
            if (tbl_Estampas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Imagen = new SelectList(db.tbl_Imagenes.Where(e => e.tbl_grupoImagen.codigo == "1").ToList<tbl_Imagenes>(), "Id", "URI", tbl_Estampas.Id_Imagen);
            return View(tbl_Estampas);
        }

        // POST: tbl_Estampas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo,Nombre,Descripcion,Id_Imagen,Visible,Eliminado")] tbl_Estampas tbl_Estampas)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (ModelState.IsValid)
            {
                tbl_Estampas.Id_Imagen = (tbl_Estampas.Id_Imagen == null) ? 1 : tbl_Estampas.Id_Imagen;
                tbl_Estampas.Eliminado = false;
                tbl_Estampas.Visible = (tbl_Estampas.Visible == null) ? false : tbl_Estampas.Visible;
                db.Entry(tbl_Estampas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Imagen = new SelectList(db.tbl_Imagenes, "Id", "Nombre", tbl_Estampas.Id_Imagen);
            return View(tbl_Estampas);
        }

        // GET: tbl_Estampas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Estampas tbl_Estampas = db.tbl_Estampas.Find(id);
            if (tbl_Estampas == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Estampas);
        }

        // POST: tbl_Estampas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {           

            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (ModelState.IsValid)
            {
                tbl_Estampas _Estampa = db.tbl_Estampas.Find(id);
                _Estampa.Eliminado = true;                
                db.Entry(_Estampa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
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
