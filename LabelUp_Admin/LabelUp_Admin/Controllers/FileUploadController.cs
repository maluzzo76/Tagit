using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using LabelUp_Admin.Models;
using System.Data;
using System.Data.Entity;
using System.Net;



namespace LabelUp_Admin.Controllers
{
    public class FileUploadController : Controller
    {
        public BusinessEntities db = new BusinessEntities();
        // GET: FileUpload
        public ActionResult Index()
        {
            ViewBag.tbl_GrupoImagen = db.tbl_grupoImagen.ToList<tbl_grupoImagen>(); 
            return View("Index");
        }

        [HttpPost]       
        public ActionResult UploadFiles(HttpPostedFileBase file, string nombre, string grupo)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    //Method 2 Get file details from HttpPostedFileBase class    

                    if (file != null)
                    {
                        string _server = Request.Path;
                        string path = Path.Combine(Server.MapPath("~/UploadImage"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        FileInfo _fileInfo = new FileInfo(path);
                         LabelUp_Admin.Models.BusinessEntities db = new Models.BusinessEntities();
                           LabelUp_Admin.Models.tbl_Imagenes _image = new Models.tbl_Imagenes();
                        _image.Nombre = nombre;
                        _image.URI = string.Format("~/UploadImage/{0}",_fileInfo.Name);
                        _image.Tamanio =(int) _fileInfo.Length;
                        _image.Extencion = _fileInfo.Extension;
                        _image.Actio = true;
                        _image.Id_GrupoImagen = int.Parse(grupo);

                        db.tbl_Imagenes.Add(_image);
                        db.SaveChanges();
                    }
                    ViewBag.tbl_GrupoImagen = db.tbl_grupoImagen.ToList<tbl_grupoImagen>();
                    ViewBag.Success = "La imagen se subio con exito.";
                }
                catch (Exception)
                {
                    ViewBag.Error = "No se pudo cargar la imagen."; ;
                }
            }
            return RedirectToAction("Index","tbl_Imagenes");
        }
    }
}