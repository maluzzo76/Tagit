using ASMercadoPago.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace LabelUp_Admin.Controllers
{
    public class TemplatesMails
    {
        public static string GetMailConfirmacionCompra(Models.tbl_Pedidos pedido)
        {
            string _host = System.Configuration.ConfigurationManager.AppSettings["host"];
            StringBuilder cuerpo = new StringBuilder(); 
            cuerpo.Append("<html>");
            cuerpo.Append("<meta charset='utf-8'>");
            cuerpo.Append("</head>");

            cuerpo.Append("<body style='font-family:Arial'>");

            cuerpo.Append("<iframe href='https://localhost:44398/tbl_Pedidos/VerPedido/109'></iframe>");

            cuerpo.Append("<div style='width:100%; background-color:#346dee'>");
            cuerpo.Append(" <div style='width:100%; text-align:center;'> ");
            cuerpo.Append("     <img src = 'http://tagit.ar/Content/imagenes/site/logo_mail.png' alt ='logo' >");
            cuerpo.Append(" </div> ");
            cuerpo.Append(" <h2 style='color:white; text-align:center;'>MUCHAS GRACIAS POR TU COMPRA</h2>  ");
            cuerpo.Append("</div> ");

            cuerpo.Append("<div <div style='width:80%'> ");
            cuerpo.Append("<p>¡Confirmamos el pago por tu compra en Tag-it! Por favor tené en cuenta que los productos personalizados los hacemos especialmente para vos y contamos con 10 días hábiles para su producción. <P>");
            cuerpo.Append("<p>Ya estamos preparando tu pedido para enviártelo. ¡Te vamos a avisar cuando esté en camino!</p>");
            cuerpo.Append("<p>Por favor fíjate el detalle debajo y controlá que todos los datos sean correctos:</p>");
            cuerpo.Append("</div> ");


            cuerpo.Append(string.Format("<a  href='{0}/tbl_Pedidos/VerPedido/{1}'>Ver Pedido</a>", _host,pedido.Id));



            cuerpo.Append("</body>");

            /*           
            cuerpo.Append("<div><h2 style='background-color:red;'>Gracias</p></div></html>");
            cuerpo.Append("</html>");
            */


            return cuerpo.ToString();

        }

        public static string GetMailContacto(string email, string nombre, string phone, string consulta)
        {
            StringBuilder cuerpo = new StringBuilder();
            cuerpo.Append(string.Format("<p>NOMBRE Y APELLIDO: {0}</p>", nombre));
            cuerpo.Append(string.Format("<p>EMAIL: {0}</p>", email));
            cuerpo.Append(string.Format("<p>TELEFONO: {0}</p>", phone));
            cuerpo.Append(string.Format("<p>CONSULTA:</p><textarea cols=\"80\" rows=\"7\">{0}</textarea>", consulta));


            return cuerpo.ToString();

        }
    }
}