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
    public class tbl_Envios_ParametrosController : CustomController
    {
        public BusinessEntities db = new BusinessEntities();

        // GET: tbl_Envios_Parametros
        public ActionResult Index(string SearchString, int pagina = 1)
        {

            if (!ValidarAdmin())
                return RoutingNoAdmin();

            var _r = from _o in db.tbl_Envios_Parametros
                     where _o.Eliminado == false
                     orderby _o.Id descending
                     select _o;


            if (!string.IsNullOrEmpty(SearchString))
            {
                _r = from _o in db.tbl_Envios_Parametros
                     where _o.Localidad.Contains(SearchString)  && _o.Eliminado == false
                     orderby _o.Id descending
                     select _o;
            }

            Pagination<tbl_Envios_Parametros> _page = new Pagination<tbl_Envios_Parametros>();
            return View(_page.paginado(_r, pagina));
        }

        // GET: tbl_Envios_Parametros/Details/5
        public ActionResult Details(int? id)
        {

            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Envios_Parametros tbl_Envios_Parametros = db.tbl_Envios_Parametros.Find(id);
            if (tbl_Envios_Parametros == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Envios_Parametros);
        }

        // GET: tbl_Envios_Parametros/Create
        public ActionResult Create()
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            return View();
        }

        // POST: tbl_Envios_Parametros/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo_Postal,Localidad,Costo_Envio")] tbl_Envios_Parametros tbl_Envios_Parametros)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (ModelState.IsValid)
            {
                tbl_Envios_Parametros.Eliminado = false;
                db.tbl_Envios_Parametros.Add(tbl_Envios_Parametros);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Envios_Parametros);
        }

        // GET: tbl_Envios_Parametros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Envios_Parametros tbl_Envios_Parametros = db.tbl_Envios_Parametros.Find(id);
            if (tbl_Envios_Parametros == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Envios_Parametros);
        }

        // POST: tbl_Envios_Parametros/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo_Postal,Localidad,Costo_Envio")] tbl_Envios_Parametros tbl_Envios_Parametros)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (ModelState.IsValid)
            {
                tbl_Envios_Parametros.Eliminado = false;
                db.Entry(tbl_Envios_Parametros).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Envios_Parametros);
        }

        // GET: tbl_Envios_Parametros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Envios_Parametros tbl_Envios_Parametros = db.tbl_Envios_Parametros.Find(id);
            if (tbl_Envios_Parametros == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Envios_Parametros);
        }

        // POST: tbl_Envios_Parametros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (ModelState.IsValid)
            {
                tbl_Envios_Parametros _Entity = db.tbl_Envios_Parametros.Find(id);
                _Entity.Eliminado = true;
                db.Entry(_Entity).State = EntityState.Modified;
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
