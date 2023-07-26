using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using LabelUp_Admin.Models;
namespace LabelUp_Admin.Controllers
{
    public class HomeController : CustomController
    {
        

        public ActionResult splash()
        {
            ViewBag.isSplash = true;
            return View();
        }

        public ActionResult Index()
        {            
            ViewBag.ProductoStore = db.tbl_Productos.Where(p => p.Eliminado == false && p.Visible == true && p.Is_Destacado == true).ToList<tbl_Productos>();

            //Prueba Mail

            /*
            IList<string> _to = new List<string>();
            _to.Add("mariano.aluzzo@alusoft.com.ar");
            SendMail(_to, "Tag IT - Muchas Gracias por su compra", TemplatesMails.GetMailConfirmacionCompra(db.tbl_Pedidos.Find(39)));
            */


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            
            return View();
        }

        public ActionResult ContactSend(string Nombre, string Email, string Phone, string Consulta)
        {
            IList<string> _to = new List<string>();
            _to.Add(System.Configuration.ConfigurationManager.AppSettings["EmailContacto"] );
            SendMail(_to, "Tag IT - Contacto Site", TemplatesMails.GetMailContacto(Email,Nombre,Phone,Consulta));

            TempData["mensajeConfirm"] = "La consulta se ha enviado con exito.";
            return RedirectToAction("Index", "Home");
        }
    }
}