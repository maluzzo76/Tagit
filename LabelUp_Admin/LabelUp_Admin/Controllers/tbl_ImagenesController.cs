using System;
using System.IO;
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
    public class tbl_ImagenesController : CustomController
    {
        public BusinessEntities db = new BusinessEntities();

        // GET: tbl_Imagenes
        public ActionResult Index(string SearchString,string SearchGrupo, int pagina = 1)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            var _r = from _i in db.tbl_Imagenes
                     where _i.Id>1
                     select _i;

            if (!string.IsNullOrEmpty(SearchString))
            {
                _r = from _i in db.tbl_Imagenes
                     where _i.Id > 1 && _i.Nombre.Contains(SearchString)
                     select _i;
            }

            if (!string.IsNullOrEmpty(SearchGrupo))
            {
                _r = from _i in _r
                     where _i.Id > 1 && _i.tbl_grupoImagen.Nombre.Contains(SearchGrupo)
                     select _i;
            }

            Pagination <tbl_Imagenes> _Page = new Pagination<tbl_Imagenes>();          
            
            return View(_Page.paginado(_r, pagina));
        }

        // GET: tbl_Imagenes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Imagenes tbl_Imagenes = db.tbl_Imagenes.Find(id);
            if (tbl_Imagenes == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Imagenes);
        }

        // GET: tbl_Imagenes/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: tbl_Imagenes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Actio,URI,Tamanio,Extencion")] tbl_Imagenes tbl_Imagenes)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (ModelState.IsValid)
            {                
                db.tbl_Imagenes.Add(tbl_Imagenes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Imagenes);
        }

        // GET: tbl_Imagenes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

             
            tbl_Imagenes tbl_Imagenes = db.tbl_Imagenes.Find(id);
            if (tbl_Imagenes == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_GrupoImagen = new SelectList(db.tbl_grupoImagen, "Id", "Nombre", tbl_Imagenes.Id_GrupoImagen);
            return View(tbl_Imagenes);
        }

        // POST: tbl_Imagenes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Actio,URI,Tamanio,Extencion, Id_GrupoImagen")] tbl_Imagenes tbl_Imagenes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Imagenes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Imagenes);
        }

        // GET: tbl_Imagenes/Delete/5
        public ActionResult Delete(int? id)
        {

            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Imagenes tbl_Imagenes = db.tbl_Imagenes.Find(id);
            if (tbl_Imagenes == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Imagenes);
        }

        // POST: tbl_Imagenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_Imagenes tbl_Imagenes = db.tbl_Imagenes.Find(id);
            try
            {
                //para habilitar el borrado se debe generar el trigger
                db.tbl_Imagenes.Remove(tbl_Imagenes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.Warning = "No es posible eliminar la imagen, la misma esta siendo utilizados por otros modulos.";
            }
            return View(tbl_Imagenes);
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
