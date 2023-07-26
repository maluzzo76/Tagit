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
using System.Net.Mail;
using System.Configuration;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace LabelUp_Admin.Controllers
{
    public class CustomController: Controller
    {
        public BusinessEntities db = new BusinessEntities();

        public bool ValidarAdmin()
        {
            
            tbl_UsuariosController _ctrol = new tbl_UsuariosController();
            if (User == null)
                return false;

            if (!_ctrol.isAdmin(User.Identity.Name))
                return false;

            return true;
        }

        public bool ValidarAdmin(string userName)
        {
            tbl_UsuariosController _ctrol = new tbl_UsuariosController();            

            if (!_ctrol.isAdmin(userName))
                return false;

            return true;
        }

        public ActionResult RoutingNoAdmin()
        {
            
            return  RedirectToAction("Index","Home");
        }

        public void NewKeySessionId()
        {
            System.Web.HttpContext.Current.Session["KeySession"] = Guid.NewGuid();
        }

        public Guid KeySessionId()
        {
            if (System.Web.HttpContext.Current.Session["KeySession"] == null)
                System.Web.HttpContext.Current.Session.Add("KeySession", Guid.NewGuid());

            return Guid.Parse(System.Web.HttpContext.Current.Session["KeySession"].ToString());
        }

        public int cartCount()
        {
            var _key = KeySessionId();
            return db.tbl_cart.Where(o=> o.keyUser == _key).ToList().Count();
        }

        public void SendMail(IList<string> to, string subject, string body)
        {
            try
            {
                string _userName = System.Configuration.ConfigurationManager.AppSettings["MailUserName"];
                string _password = System.Configuration.ConfigurationManager.AppSettings["MailPassword"];
                string _host = System.Configuration.ConfigurationManager.AppSettings["MailHost"];
                int _port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MailPort"]);
                bool _Ssl = (System.Configuration.ConfigurationManager.AppSettings["MailSsl"].ToString().Equals("0")) ? false : true;

                MailMessage correo = new MailMessage();
                correo.From = new MailAddress(_userName, "Tag It - Store", System.Text.Encoding.UTF8);//Correo de salida

                //Set correo destino
                foreach (string _to in to)
                {
                    correo.To.Add(_to);
                }

                correo.Subject = subject;

                correo.Body = body;

                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;

                smtp.Host = _host;
                smtp.Port = _port;
                smtp.Credentials = new System.Net.NetworkCredential(_userName, _password);//Cuenta de correo

                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtp.EnableSsl = _Ssl;//True si el servidor de correo permite ssl
                smtp.Send(correo);
            }
            catch (Exception ex)
            {
                TempData["mensajeWaring"] = "Error en el envio de correo ";
            }
            finally { }
        }


    }
}