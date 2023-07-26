using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LabelUp_Admin.Models;

namespace LabelUp_Admin.Controllers
{
    public class tbl_PlantillasController : CustomController
    {
        public BusinessEntities db = new BusinessEntities();

        // GET: tbl_Plantillas
        public ActionResult Index(string SearchString, int pagina = 1)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            var _r = from _o in db.tbl_Plantillas.Include(t => t.tbl_Imagenes).Include(t => t.tbl_Imagenes1)
                     where _o.Eliminado == false
                     select _o;

            if (!string.IsNullOrEmpty(SearchString))
            {
                _r = from _o in _r
                     where _o.Codigo.Contains(SearchString) || _o.Nombre.Contains(SearchString)
                     select _o;
            }

            Pagination<tbl_Plantillas> _page = new Pagination<tbl_Plantillas>();

            return View(_page.paginado(_r,pagina));
        }

        // GET: tbl_Plantillas/Details/5
        public ActionResult Details(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Plantillas tbl_Plantillas = db.tbl_Plantillas.Find(id);
            if (tbl_Plantillas == null)
            {
                return HttpNotFound();
            }

            ViewBag.Id_imagenPlantilla = new SelectList(db.tbl_Imagenes.Where(e => e.tbl_grupoImagen.codigo == "4").ToList<tbl_Imagenes>(), "Id", "URI", tbl_Plantillas.Id_imagenPlantilla);
            return View(tbl_Plantillas);
        }

        // GET: tbl_Plantillas/Create
        public ActionResult Create()
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            ViewBag.Id_imagen = new SelectList(db.tbl_Imagenes.Where(e => e.tbl_grupoImagen.codigo == "3").ToList<tbl_Imagenes>(), "Id", "URI");
            ViewBag.Id_imagenPlantilla = new SelectList(db.tbl_Imagenes.Where(e => e.tbl_grupoImagen.codigo == "3").ToList<tbl_Imagenes>(), "Id", "URI");
            return View();
        }

        // POST: tbl_Plantillas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,Nombre,Descripcion,Id_imagen,Id_imagenPlantilla,Visible,Eliminado")] tbl_Plantillas tbl_Plantillas)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (ModelState.IsValid)
            {
                tbl_Plantillas.Id_imagen = (tbl_Plantillas.Id_imagen == null) ? 1 : tbl_Plantillas.Id_imagen;
                tbl_Plantillas.Id_imagenPlantilla = (tbl_Plantillas.Id_imagenPlantilla == null) ? 1 : tbl_Plantillas.Id_imagenPlantilla;
                tbl_Plantillas.Eliminado = false;
                db.tbl_Plantillas.Add(tbl_Plantillas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_imagen = new SelectList(db.tbl_Imagenes.Where(e => e.tbl_grupoImagen.codigo == "3").ToList<tbl_Imagenes>(), "Id", "Nombre", tbl_Plantillas.Id_imagen);
            ViewBag.Id_imagenPlantilla = new SelectList(db.tbl_Imagenes.Where(e => e.tbl_grupoImagen.codigo == "3").ToList<tbl_Imagenes>(), "Id", "Nombre", tbl_Plantillas.Id_imagenPlantilla);
            return View(tbl_Plantillas);
        }

        // GET: tbl_Plantillas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Plantillas tbl_Plantillas = db.tbl_Plantillas.Find(id);
            if (tbl_Plantillas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_imagen = new SelectList(db.tbl_Imagenes.Where(e => e.tbl_grupoImagen.codigo == "3").ToList<tbl_Imagenes>(), "Id", "URI", tbl_Plantillas.Id_imagen);
            ViewBag.Id_imagenPlantilla = new SelectList(db.tbl_Imagenes.Where(e => e.tbl_grupoImagen.codigo == "3").ToList<tbl_Imagenes>(), "Id", "URI", tbl_Plantillas.Id_imagenPlantilla);
            return View(tbl_Plantillas);
        }

        public ActionResult SetEstampa(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            tbl_Plantillas_Estampas tbl_PE = db.tbl_Plantillas_Estampas.Find(id);
            tbl_PE.Activo = (tbl_PE.Activo) ? false : true;

            db.Entry(tbl_PE).State = EntityState.Modified;
            db.SaveChanges();

            tbl_Plantillas_Estampas tbl_plantillas_estampas = db.tbl_Plantillas_Estampas.Find(tbl_PE.Id_Plantilla);
            return RedirectToAction(string.Format("Details/{0}", tbl_PE.Id_Plantilla), "tbl_Plantillas");
        }

        public ActionResult SetImagenPlantilla(int? id, int? idimg)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            tbl_Plantillas_Estampas tbl_PE = db.tbl_Plantillas_Estampas.Find(id);
            tbl_PE.Id_ImagenPlantilla = idimg.Value;

            db.Entry(tbl_PE).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction(string.Format("Details/{0}", tbl_PE.Id_Plantilla), "tbl_Plantillas");
        }

        // POST: tbl_Plantillas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo,Nombre,Descripcion,Id_imagen,Id_imagenPlantilla,Visible,Eliminado")] tbl_Plantillas tbl_Plantillas)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (ModelState.IsValid)
            {
                tbl_Plantillas.Id_imagen = (tbl_Plantillas.Id_imagen == null) ? 1 : tbl_Plantillas.Id_imagen;
                tbl_Plantillas.Id_imagenPlantilla = (tbl_Plantillas.Id_imagenPlantilla == null) ? 1 : tbl_Plantillas.Id_imagenPlantilla;
                tbl_Plantillas.Eliminado = false;
                tbl_Plantillas.Visible = (tbl_Plantillas.Visible == null) ? false : tbl_Plantillas.Visible;

                db.Entry(tbl_Plantillas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_imagen = new SelectList(db.tbl_Imagenes, "Id", "Nombre", tbl_Plantillas.Id_imagen);
            ViewBag.Id_imagenPlantilla = new SelectList(db.tbl_Imagenes, "Id", "Nombre", tbl_Plantillas.Id_imagenPlantilla);
            return View(tbl_Plantillas);
        }

        // GET: tbl_Plantillas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Plantillas tbl_Plantillas = db.tbl_Plantillas.Find(id);
            if (tbl_Plantillas == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Plantillas);
        }

        // POST: tbl_Plantillas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            tbl_Plantillas tbl_plantillas = db.tbl_Plantillas.Find(id);
            tbl_plantillas.Eliminado = true;

            db.Entry(tbl_plantillas).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Formato(int pid)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();
            tbl_Plantillas tbl_Plantillas = db.tbl_Plantillas.Find(pid);
            List<tbl_PrametrosPersonalizacion> _lstParametros = db.tbl_PrametrosPersonalizacion.Where(o => o.Id_Plantilla == pid).ToList<tbl_PrametrosPersonalizacion>();
            ViewBag.lstFormato = _lstParametros;
            getScriptFormato(_lstParametros);
            
            return View(tbl_Plantillas);
        }
        
        public ActionResult DeleteParametro(int idp, int pid)
        {
           
            try
            {
                SqlConnection _oconn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                string _query = string.Format("delete tbl_PrametrosPersonalizacion where id = {0}", idp);
                using (SqlCommand _sqlCommand = new SqlCommand(_query,_oconn))
                {
                    _sqlCommand.Connection.Open();
                    _sqlCommand.ExecuteNonQuery();
                    _sqlCommand.Connection.Close();
                    _sqlCommand.Connection.Dispose();
                }                  
            }
            catch (Exception ex)
            {
                TempData["mensajeWaring"] = ex.Message;
                return RedirectToAction("Formato", "tbl_Plantillas", new { pid = pid });
            }

            return RedirectToAction("Formato", "tbl_Plantillas", new { pid = pid });

        }

        public ActionResult UpdateCoordenadas(int id,int x, int y, int w, int h )
        {
            tbl_PrametrosPersonalizacion _param = db.tbl_PrametrosPersonalizacion.Find(id);
            _param.X = x;
            _param.Y = y;
            _param.W = w;
            _param.H = h;
            db.Entry(_param).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Formato", "tbl_Plantillas", new { pid = _param.Id_Plantilla });
        }

        public ActionResult CreateFormato([Bind(Include = "Id, Id_Plantilla, X,Y,W,H,CuadroTexto,Zoom")] tbl_PrametrosPersonalizacion tbl_PrametrosPersonalizacion)
        {
        if (!ValidarAdmin())
            return RoutingNoAdmin();

            db.tbl_PrametrosPersonalizacion.Add(tbl_PrametrosPersonalizacion);
            db.SaveChanges();
            return RedirectToAction("Formato","tbl_Plantillas",new { pid = tbl_PrametrosPersonalizacion.Id_Plantilla });
        }

        void getScriptFormato(List<tbl_PrametrosPersonalizacion> parametros)
        {
            string _script = "";

            foreach (tbl_PrametrosPersonalizacion item in parametros)
            {

                int _fsize = convertZoom(item.Zoom.Value);
                _script = _script + string.Format("creatDivMt({0}, {1}, {2}, {3},{4},{5},{6});", item.X, item.Y, item.W, item.H, item.CuadroTexto,_fsize, item.Id);
            }
          

            ViewBag.scriptDib = _script;
        }

        int convertZoom(int zoom)
        {
            int result = 23;
            
            decimal _zoom = decimal.Parse("1.24");

            if (zoom < 100)
            {
                _zoom =decimal.Divide(100,zoom);

                 result = (int)(23  / _zoom);
            }


            return result;
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
