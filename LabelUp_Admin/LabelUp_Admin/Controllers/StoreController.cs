using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.Mvc;
using LabelUp_Admin.Models;

namespace LabelUp_Admin.Controllers
{
    public class StoreController : CustomController
    {


        // GET: Store
        public ActionResult Index()
        {
            ViewBag.lstProductos = (List<tbl_Productos>)db.tbl_Productos.Where(p => p.Eliminado == false & p.Visible == true).ToList<tbl_Productos>();
            return View();
        }

        public ActionResult step1(int id, int pagina = 0)
        {
            ViewBag.IdProducto = id;
            ViewBag.ProductoName = db.tbl_Productos.Find(id).Nombre;
            ViewBag.PageIndex = pagina;
            ViewBag.PageSiguienteVIsible = true;
            ViewBag.PageAnteriorVIsible = true;




            List<tbl_Productos_Plantillas> _r = (List<tbl_Productos_Plantillas>)db.tbl_Productos_Plantillas.Include(o => o.tbl_Plantillas).Where(p => p.Activo == true & p.Eliminado == false & p.Id_Producto == id).ToList<tbl_Productos_Plantillas>();
            List<tbl_Productos_Plantillas> _result = new List<tbl_Productos_Plantillas>();

            if (pagina == 0)
                ViewBag.PageAnteriorVIsible = false;

            if (pagina >= _r.Count() / 4)
                ViewBag.PageSiguienteVIsible = false;

            if (_r.Count() > 0)
            {
                for (int index = pagina; index < (pagina + 4); index++)
                {
                    if (index < _r.Count())
                    {
                        _result.Add(_r[index]);
                    }
                }
            }

            ViewBag.lstPlantillaProducto = _result;

            return View();
        }

        public ActionResult step2(int? pid, int? plid)
        {
            if (TempData.Count > 0)
            {
                pid= int.Parse(TempData["IdProducto"].ToString());
                plid = int.Parse(TempData["IdPlantilla"].ToString());

            }
            if (plid == null)
            {
                TempData["mensajeConfirm"] = null;                
                TempData["mensajeWaring"] = "Debe seleccionar una plantilla";
                return RedirectToAction(string.Concat("step1/", pid), "Store");
            }

            ViewBag.IdProducto = pid;
            ViewBag.IdPlantilla = plid;
            ViewBag.ProductoName = db.tbl_Productos.Find(pid).Nombre;
            ViewBag.PlantillaName = db.tbl_Plantillas.Find(plid).Nombre;
            ViewBag.lstvGrupoEstampas = (List<v_grupoEstampaPlantilla>)db.v_grupoEstampaPlantilla.Where(g => g.Id_Producto == pid & g.Id_Plantilla == plid).ToList<v_grupoEstampaPlantilla>();
            return View();
        }


        public ActionResult step3(int pid, int plid, int? geid)
        {
            if (geid == null)
            {
                ViewBag.IdProducto = pid;
                ViewBag.IdPlantilla = plid;
                TempData["mensajeConfirm"] = null;
                TempData["mensajeWaring"] = "Debe seleccionar una estampa";
                return RedirectToAction(string.Concat("step2/", pid), "Store");
            }

            ViewBag.IdProducto = pid;
            ViewBag.IdPlantilla = plid;
            ViewBag.IdGrupoEstampa = geid;
            ViewBag.ProductoName = db.tbl_Productos.Find(pid).Nombre;
            ViewBag.PlantillaName = db.tbl_Plantillas.Find(plid).Nombre;
            ViewBag.lstvEstampas = (List<v_EstampaPlantillaGrupoEstampa>)db.v_EstampaPlantillaGrupoEstampa.Where(e => e.Id_GrupoEstampa == geid & e.Id_Plantilla == plid).ToList<v_EstampaPlantillaGrupoEstampa>();

            return View();
        }

        public ActionResult step4(int pid, int plid, int? eid)
        {
            if (eid == null)
            {
                TempData["IdProducto"] = pid;
                TempData["IdPlantilla"] = plid;
                TempData["mensajeConfirm"] = null;
                TempData["mensajeWaring"] = "Debe seleccionar una estampa";
                return RedirectToAction("step2", "Store");
            }

            ViewBag.IdProducto = pid;
            ViewBag.IdPlantilla = plid;
            ViewBag.IdEstampa = eid;
            ViewBag.ProductoName = db.tbl_Productos.Find(pid).Nombre;
            ViewBag.ProductoDescripcion = db.tbl_Productos.Find(pid).Descripcion;
            tbl_Plantillas_Estampas _plantillaEstampa = ((List<tbl_Plantillas_Estampas>)db.tbl_Plantillas_Estampas.Where(e => e.Id_Plantilla == plid & e.Id_Estampa == eid).ToList<tbl_Plantillas_Estampas>())[0];

            List<tbl_PrametrosPersonalizacion> _lstParametros = db.tbl_PrametrosPersonalizacion.Where(o => o.Id_Plantilla == plid).ToList<tbl_PrametrosPersonalizacion>();
            ViewBag.lstFormato = _lstParametros;

            List<tbl_Productos_Plantillas> _precio = db.tbl_Productos_Plantillas.Where(p => p.Id_Producto == pid && p.Id_Plantilla == plid).ToList<tbl_Productos_Plantillas>();

            ViewBag.param1 = "visibility:hidden; height:1px;";
            ViewBag.param2 = "visibility:hidden; height:1px;";
            ViewBag.param3 = "visibility:hidden; height:1px;";
            ViewBag.param4 = "visibility:hidden; height:1px;";
            ViewBag.color = "visibility:hidden; height:1px;";
            bool _isPersonaizable = false;

            if (_lstParametros.Where(p => p.CuadroTexto == 1).Count() > 0)
            {
                ViewBag.param1 = "visibility:visible; height:60px;";
                _isPersonaizable = true;
            }

            if (_lstParametros.Where(p => p.CuadroTexto == 2).Count() > 0)
            {
                ViewBag.param2 = "visibility:visible; height:60px;";
                _isPersonaizable = true;
            }

            if (_lstParametros.Where(p => p.CuadroTexto == 3).Count() > 0)
            {
                ViewBag.param3 = "visibility:visible; height:60px;";
                _isPersonaizable = true;
            }

            if (_lstParametros.Where(p => p.CuadroTexto == 4).Count() > 0)
            {
                ViewBag.param4 = "visibility:visible; height:60px;";
                _isPersonaizable = true;
            }

            if (_isPersonaizable == true)
                ViewBag.color = "visibility:visible; height:60px;";

            ViewBag.Precio = "0.00";
            if (_precio.Count > 0)
                ViewBag.Precio = decimal.Round(_precio[0].Precio.Value, 2).ToString();

            getScriptFormato(_lstParametros);
            return View(_plantillaEstampa);
        }

        public ActionResult addToCart(int idPro, int idPlan, int idEst, int idplaEst, string param1, string param2, string param3, string param4, int paramSize1, int paramSize2, int paramSize3, int paramSize4, string fontName, string fontColor, decimal precioUnitario, int cantidad, decimal total)
        {
            ViewBag.IdProducto = idPro;
            ViewBag.IdPlantilla = idPlan;
            ViewBag.IdEstampa = idEst;
            ViewBag.ProductoName = db.tbl_Productos.Find(idPro).Nombre;
            tbl_Plantillas_Estampas _plantillaEstampa = ((List<tbl_Plantillas_Estampas>)db.tbl_Plantillas_Estampas.Where(e => e.Id_Plantilla == idPlan & e.Id_Estampa == idEst).ToList<tbl_Plantillas_Estampas>())[0];

            tbl_cart _newCart = new tbl_cart();
            _newCart.Fecha = DateTime.Now;
            _newCart.Id_Estado = 1;
            _newCart.Id_Usuario = null;
            _newCart.keyUser = KeySessionId();
            _newCart.Id_Producto = idPro;
            _newCart.Id_Plantilla = idPlan;
            _newCart.Id_PlantillaEstampa = idplaEst;
            _newCart.Param1 = param1;
            _newCart.Param2 = param2;
            _newCart.Param3 = param3;
            _newCart.Param4 = param4;
            _newCart.ParamSize1 = paramSize1;
            _newCart.ParamSize2 = paramSize2;
            _newCart.ParamSize3 = paramSize3;
            _newCart.ParamSize4 = paramSize4;
            _newCart.FontName = fontName;
            _newCart.FontColor = fontColor;
            _newCart.PrecioUnitario = precioUnitario;
            _newCart.Cantidad = cantidad;
            _newCart.Total = precioUnitario * cantidad;

            db.tbl_cart.Add(_newCart);
            db.SaveChanges();
            return RedirectToAction("Index", "tbl_Cart");
        }

        void getScriptFormato(List<tbl_PrametrosPersonalizacion> parametros)
        {
            string _script = "";
            string _scriptWrite = "";
            int _index = 0;
            foreach (tbl_PrametrosPersonalizacion item in parametros)
            {
                int _fsize = convertZoom(item.Zoom.Value);
                string _zoom = "1";

                if (item.Zoom.Value < 100)
                    _zoom = string.Format("0.{0}", item.Zoom.Value);

                _scriptWrite = _scriptWrite + getScriptWriteText(item.CuadroTexto.Value, _index, _zoom);
                _script = _script + string.Format("creatDivMt({0}, {1}, {2}, {3},{4},{5},{6});", item.X, item.Y, item.W, item.H, item.CuadroTexto, _fsize, _index);
                _index++;
            }


            ViewBag.scriptWriteText = _scriptWrite;
            ViewBag.scriptDib = _script;
        }

        string getScriptWriteText(int cuadroTexto, int index, string zoom)
        {

            string _script = "$('#vText{1}_{0}').text($('#Param{0}').val()); " +
        "$('#vText{1}_{0}').css('font-size', convertZoom({2},$('#paramSize{0}').val()) + 'pt'); " +
        "$('#vText{1}_{0}').css('color', $('#FontColor').val()); " +
        "$('#vText{1}_{0}').css('font-family', $('#FontName').val()); " +
        "if (_BoolParam{0}) $('#vText{1}_{0}').css('font-weight', 'bold'); else $('#vText{1}_{0}').css('font-weight', 'normal');";

            return string.Format(_script, cuadroTexto, index, zoom);

        }


        int convertZoom(int zoom)
        {
            decimal _zoom = 1;

            if (zoom < 100)
            {
                _zoom = decimal.Parse(string.Format("0,{0}", zoom));

                return (int)((decimal)24 * _zoom);
            }


            return 24;
        }
    }
}