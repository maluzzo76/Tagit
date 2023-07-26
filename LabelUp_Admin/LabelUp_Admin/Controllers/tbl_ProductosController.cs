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
    public class tbl_ProductosController : CustomController
    {
        public BusinessEntities db = new BusinessEntities();

        // GET: tbl_Productos
        public ActionResult Index(string SearchString, int pagina = 1 )
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            var _r = from _o in db.tbl_Productos.Include(t => t.tbl_Imagenes)
                     where _o.Eliminado == false
                     select _o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                _r = from _o in _r
                     where _o.Codigo.Contains(SearchString) || _o.Nombre.Contains(SearchString)
                     select _o;
            }

            Pagination<tbl_Productos> _page = new Pagination<tbl_Productos>();

            return View(_page.paginado(_r,pagina));
        }        

        // GET: tbl_Productos/Details/5
        public ActionResult Details(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Productos tbl_Productos = db.tbl_Productos.Find(id);
            if (tbl_Productos == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Productos);
        }

        // GET: tbl_Productos/Create
        public ActionResult Create()
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            ViewBag.Id_Imagen = new SelectList(db.tbl_Imagenes.Where(e => e.tbl_grupoImagen.codigo == "5").ToList<tbl_Imagenes>(), "Id", "URI");
            return View();
        }

        // POST: tbl_Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,Nombre,Descripcion,Id_Imagen,Visible,Eliminado,Is_Destacado")] tbl_Productos tbl_Productos)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (ModelState.IsValid)
            {
                tbl_Productos.Id_Imagen = (tbl_Productos.Id_Imagen == null) ? 1 : tbl_Productos.Id_Imagen;
                tbl_Productos.Eliminado = false;
                tbl_Productos.Visible = (tbl_Productos.Visible == null) ? false : tbl_Productos.Visible;
                tbl_Productos.Is_Destacado = (tbl_Productos.Is_Destacado == null) ? false : tbl_Productos.Is_Destacado;
                db.tbl_Productos.Add(tbl_Productos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Imagen = new SelectList(db.tbl_Imagenes.Where(e => e.tbl_grupoImagen.codigo == "5").ToList<tbl_Imagenes>(), "Id", "Nombre", tbl_Productos.Id_Imagen);
            return View(tbl_Productos);
        }

        // GET: tbl_Productos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Productos tbl_Productos = db.tbl_Productos.Find(id);

            if (tbl_Productos == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Imagen = new SelectList(db.tbl_Imagenes.Where(e => e.tbl_grupoImagen.codigo == "5").ToList<tbl_Imagenes>(), "Id", "Uri", tbl_Productos.Id_Imagen);
            return View(tbl_Productos);
        }

        public ActionResult SetPlntilla(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            tbl_Productos_Plantillas tbl_grupo = db.tbl_Productos_Plantillas.Find(id);
            tbl_grupo.Activo = (tbl_grupo.Activo) ? false : true;

            db.Entry(tbl_grupo).State = EntityState.Modified;
            db.SaveChanges();
            
            return RedirectToAction(string.Format("Details/{0}", tbl_grupo.Id_Producto), "tbl_Productos");
        }

        public ActionResult SetPrecio(int? id, decimal costo)
        {
            
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            tbl_Productos_Plantillas tbl_grupo = db.tbl_Productos_Plantillas.Find(id);
            tbl_grupo.Precio = costo;

            db.Entry(tbl_grupo).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction(string.Format("Details/{0}", tbl_grupo.Id_Producto), "tbl_Productos");
        }

        // POST: tbl_Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo,Nombre,Descripcion,Id_Imagen,Visible,Eliminado,Is_Destacado")] tbl_Productos tbl_Productos)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (ModelState.IsValid)
            {
                tbl_Productos.Id_Imagen = (tbl_Productos.Id_Imagen == null) ? 1 : tbl_Productos.Id_Imagen;
                tbl_Productos.Eliminado = false;
                tbl_Productos.Visible = (tbl_Productos.Visible == null) ? false : tbl_Productos.Visible;
                tbl_Productos.Is_Destacado = (tbl_Productos.Is_Destacado == null) ? false : tbl_Productos.Is_Destacado;
                db.Entry(tbl_Productos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Imagen = new SelectList(db.tbl_Imagenes.Where(e => e.tbl_grupoImagen.codigo == "5").ToList<tbl_Imagenes>(), "Id", "Nombre", tbl_Productos.Id_Imagen);
            return View(tbl_Productos);
        }

        // GET: tbl_Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Productos tbl_Productos = db.tbl_Productos.Find(id);
            if (tbl_Productos == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Productos);
        }

        // POST: tbl_Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            tbl_Productos tbl_Productos = db.tbl_Productos.Find(id);
            tbl_Productos.Eliminado = true;
            db.Entry(tbl_Productos).State = EntityState.Modified;
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
