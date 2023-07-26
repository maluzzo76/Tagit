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
    public class tbl_Grupo_EstampasController : CustomController
    {
        public BusinessEntities db = new BusinessEntities();

        // GET: tbl_Grupo_Estampas
        public ActionResult Index(string SearchString, int pagina = 1)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            var _r = from _o in db.tbl_Grupo_Estampas.Include(t => t.tbl_Imagenes)
                     where _o.Eliminado == false
                     select _o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                _r = from _o in _r
                     where _o.Nombre.Contains(SearchString) || _o.Codigo.Contains(SearchString)
                     select _o;
            }

            Pagination<tbl_Grupo_Estampas> _page = new Pagination<tbl_Grupo_Estampas>();
            return View(_page.paginado(_r,pagina));
        }

        // GET: tbl_Grupo_Estampas/Details/5
        public ActionResult Details(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Grupo_Estampas tbl_Grupo_Estampas = db.tbl_Grupo_Estampas.Find(id);
            if (tbl_Grupo_Estampas == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Grupo_Estampas);
        }

        // GET: tbl_Grupo_Estampas/Create
        public ActionResult Create()
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

           
            ViewBag.Id_Imagen = new SelectList(db.tbl_Imagenes.Where(e=> e.tbl_grupoImagen.codigo=="2").ToList<tbl_Imagenes>()  , "Id", "URI");
            return View();
        }

        // POST: tbl_Grupo_Estampas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,Nombre,Descripcion,Id_Imagen,Visible,Eliminado")] tbl_Grupo_Estampas tbl_Grupo_Estampas)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (ModelState.IsValid)
            {
                tbl_Grupo_Estampas.Id_Imagen = (tbl_Grupo_Estampas.Id_Imagen == null) ? 1 : tbl_Grupo_Estampas.Id_Imagen;
                tbl_Grupo_Estampas.Eliminado = false;
                db.tbl_Grupo_Estampas.Add(tbl_Grupo_Estampas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Imagen = new SelectList(db.tbl_Imagenes.Where(e => e.tbl_grupoImagen.codigo == "2").ToList<tbl_Imagenes>(), "Id", "Nombre", tbl_Grupo_Estampas.Id_Imagen);
            return View(tbl_Grupo_Estampas);
        }

        public ActionResult SetEstampa(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            tbl_Estampa_GrupoEstampa tbl_grupo = db.tbl_Estampa_GrupoEstampa.Find(id);
            tbl_grupo.Activo = (tbl_grupo.Activo) ? false : true;

            db.Entry(tbl_grupo).State = EntityState.Modified;
            db.SaveChanges();

            tbl_Grupo_Estampas tbl_Grupo_Estampas = db.tbl_Grupo_Estampas.Find(tbl_grupo.Id_GrupoEstampa);
            return RedirectToAction(string.Format("Details/{0}", tbl_grupo.Id_GrupoEstampa), "tbl_Grupo_Estampas");
        }


        // GET: tbl_Grupo_Estampas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Grupo_Estampas tbl_Grupo_Estampas = db.tbl_Grupo_Estampas.Find(id);
            if (tbl_Grupo_Estampas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Imagen = new SelectList(db.tbl_Imagenes.Where(e => e.tbl_grupoImagen.codigo == "2").ToList<tbl_Imagenes>(), "Id", "URI", tbl_Grupo_Estampas.Id_Imagen);
            return View(tbl_Grupo_Estampas);
        }

        // POST: tbl_Grupo_Estampas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo,Nombre,Descripcion,Id_Imagen,Visible,Eliminado")] tbl_Grupo_Estampas tbl_Grupo_Estampas)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (ModelState.IsValid)
            {
                tbl_Grupo_Estampas.Id_Imagen = (tbl_Grupo_Estampas.Id_Imagen == null) ? 1 : tbl_Grupo_Estampas.Id_Imagen;
                tbl_Grupo_Estampas.Eliminado = false;
                tbl_Grupo_Estampas.Visible = (tbl_Grupo_Estampas.Visible == null) ? false : tbl_Grupo_Estampas.Visible;
               
                db.Entry(tbl_Grupo_Estampas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Imagen = new SelectList(db.tbl_Imagenes, "Id", "Nombre", tbl_Grupo_Estampas.Id_Imagen);
            return View(tbl_Grupo_Estampas);
        }

        // GET: tbl_Grupo_Estampas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Grupo_Estampas tbl_Grupo_Estampas = db.tbl_Grupo_Estampas.Find(id);
            if (tbl_Grupo_Estampas == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Grupo_Estampas);
        }

        // POST: tbl_Grupo_Estampas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            tbl_Grupo_Estampas tbl_Grupo_Estampas = db.tbl_Grupo_Estampas.Find(id);
            tbl_Grupo_Estampas.Eliminado = true;
            db.Entry(tbl_Grupo_Estampas).State = EntityState.Modified;
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
