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
    public class tbl_cartController : CustomController
    {

        // GET: tbl_cart
        public ActionResult Index()
        {
            var _KeySession = KeySessionId();
            var tbl_cart = db.tbl_cart.Include(t => t.tbl_Estampas).Include(t => t.tbl_Estados_Pedido).Include(t => t.tbl_Plantillas).Include(t => t.tbl_Productos).Include(t => t.tbl_Usuarios).Where(c=> c.keyUser == _KeySession) ;

            ViewBag.Lst_Envio_Parametro = db.tbl_Envios_Parametros.Where(p=>p.Eliminado==false).ToList<tbl_Envios_Parametros>();

            return View(tbl_cart.ToList());
        }

        // GET: tbl_cart/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_cart tbl_cart = db.tbl_cart.Find(id);
            if (tbl_cart == null)
            {
                return HttpNotFound();
            }
            return View(tbl_cart);
        }

        // GET: tbl_cart/Create
        public ActionResult Create()
        {
            ViewBag.Id_Estampa = new SelectList(db.tbl_Estampas, "Id", "Codigo");
            ViewBag.Id_Estado = new SelectList(db.tbl_Estados_Pedido, "Id", "Estado");
            ViewBag.Id_Plantilla = new SelectList(db.tbl_Plantillas, "Id", "Codigo");
            ViewBag.Id_Producto = new SelectList(db.tbl_Productos, "Id", "Codigo");
            ViewBag.Id_Usuario = new SelectList(db.tbl_Usuarios, "Id", "Email");
            return View();
        }

        // POST: tbl_cart/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fecha,Id_Estado,Id_Usuario,keyUser,Id_Producto,Id_Plantilla,Id_Estampa,Param1,Param2,Param3,Param4,ParamSize1,ParamSize2,ParamSize3,ParamSize4,FontName,FontColor,PrecioUnitario,Cantidad,Total")] tbl_cart tbl_cart)
        {
            if (ModelState.IsValid)
            {
                db.tbl_cart.Add(tbl_cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Estampa = new SelectList(db.tbl_Estampas, "Id", "Codigo", tbl_cart.Id_Estampa);
            ViewBag.Id_Estado = new SelectList(db.tbl_Estados_Pedido, "Id", "Estado", tbl_cart.Id_Estado);
            ViewBag.Id_Plantilla = new SelectList(db.tbl_Plantillas, "Id", "Codigo", tbl_cart.Id_Plantilla);
            ViewBag.Id_Producto = new SelectList(db.tbl_Productos, "Id", "Codigo", tbl_cart.Id_Producto);
            ViewBag.Id_Usuario = new SelectList(db.tbl_Usuarios, "Id", "Email", tbl_cart.Id_Usuario);
            return View(tbl_cart);
        }

        // GET: tbl_cart/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_cart tbl_cart = db.tbl_cart.Find(id);
            if (tbl_cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Estampa = new SelectList(db.tbl_Estampas, "Id", "Codigo", tbl_cart.Id_Estampa);
            ViewBag.Id_Estado = new SelectList(db.tbl_Estados_Pedido, "Id", "Estado", tbl_cart.Id_Estado);
            ViewBag.Id_Plantilla = new SelectList(db.tbl_Plantillas, "Id", "Codigo", tbl_cart.Id_Plantilla);
            ViewBag.Id_Producto = new SelectList(db.tbl_Productos, "Id", "Codigo", tbl_cart.Id_Producto);
            ViewBag.Id_Usuario = new SelectList(db.tbl_Usuarios, "Id", "Email", tbl_cart.Id_Usuario);
            return View(tbl_cart);
        }

        // POST: tbl_cart/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Fecha,Id_Estado,Id_Usuario,keyUser,Id_Producto,Id_Plantilla,Id_Estampa,Param1,Param2,Param3,Param4,ParamSize1,ParamSize2,ParamSize3,ParamSize4,FontName,FontColor,PrecioUnitario,Cantidad,Total")] tbl_cart tbl_cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Estampa = new SelectList(db.tbl_Estampas, "Id", "Codigo", tbl_cart.Id_Estampa);
            ViewBag.Id_Estado = new SelectList(db.tbl_Estados_Pedido, "Id", "Estado", tbl_cart.Id_Estado);
            ViewBag.Id_Plantilla = new SelectList(db.tbl_Plantillas, "Id", "Codigo", tbl_cart.Id_Plantilla);
            ViewBag.Id_Producto = new SelectList(db.tbl_Productos, "Id", "Codigo", tbl_cart.Id_Producto);
            ViewBag.Id_Usuario = new SelectList(db.tbl_Usuarios, "Id", "Email", tbl_cart.Id_Usuario);
            return View(tbl_cart);
        }

        // GET: tbl_cart/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_cart tbl_cart = db.tbl_cart.Find(id);
            if (tbl_cart == null)
            {
                return HttpNotFound();
            }
            return View(tbl_cart);
        }

        // POST: tbl_cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_cart tbl_cart = db.tbl_cart.Find(id);
            db.tbl_cart.Remove(tbl_cart);
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
