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
    public class tbl_UsuariosController : Controller
    {
        public BusinessEntities db = new BusinessEntities();

        // GET: tbl_Usuarios
        public ActionResult Index()
        {
            return View(db.tbl_Usuarios.ToList());
        }

        public bool isAdmin(string email)
        {
            var _r = from _u in db.tbl_Usuarios.ToList()
                     where _u.Email.Equals(email) && _u.Activo.Equals(true) && _u.Is_Admin.Equals(true)
                     select _u;

            if (string.IsNullOrEmpty(email))
                return false;

            if (_r.Count() > 0)
                return true;

            return false;
        }

        // GET: tbl_Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Usuarios tbl_Usuarios = db.tbl_Usuarios.Find(id);
            if (tbl_Usuarios == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Usuarios);
        }

        // GET: tbl_Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tbl_Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,Activo,Is_Admin")] tbl_Usuarios tbl_Usuarios)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Usuarios.Add(tbl_Usuarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Usuarios);
        }

        // GET: tbl_Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Usuarios tbl_Usuarios = db.tbl_Usuarios.Find(id);
            if (tbl_Usuarios == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Usuarios);
        }

        // POST: tbl_Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Activo,Is_Admin")] tbl_Usuarios tbl_Usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Usuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Usuarios);
        }

        // GET: tbl_Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Usuarios tbl_Usuarios = db.tbl_Usuarios.Find(id);
            if (tbl_Usuarios == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Usuarios);
        }

        // POST: tbl_Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_Usuarios tbl_Usuarios = db.tbl_Usuarios.Find(id);
            db.tbl_Usuarios.Remove(tbl_Usuarios);
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
