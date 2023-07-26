using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using LabelUp_Admin.Models;
using System.Net.Http;
using ASMercadoPago.Model;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNet;
using System.Text.Encodings;
using System.Text;

namespace LabelUp_Admin.Controllers
{
    public class tbl_PedidosController : CustomController
    {
        public BusinessEntities db = new BusinessEntities();

        // GET: tbl_Pedidos


        public ActionResult Index(int? id, int pagina = 1)
        {
            if (!ValidarAdmin())
                return RoutingNoAdmin();

            var tbl_Pedidos = db.tbl_Pedidos.Include(t => t.tbl_Estados_Pedido).Include(t => t.tbl_Usuarios);

            if (id != null)
            {
                var _keyUser = KeySessionId();
                tbl_Pedidos = db.tbl_Pedidos.Include(t => t.tbl_Estados_Pedido).Include(t => t.tbl_Usuarios).Where(c => c.Id == id);
            }

            var _r = tbl_Pedidos.OrderByDescending(o => o.Fecha);

            Pagination<tbl_Pedidos> _page = new Pagination<tbl_Pedidos>();

            return View(_page.paginado(_r, pagina));
        }

        public ActionResult MercadoPagoResponse(int? id)
        {
            tbl_Pedidos _newPedido = db.tbl_Pedidos.Find(id);
            _newPedido.MercadoPagoId = Request.QueryString["payment_id"];
            _newPedido.MercadoPagoStatus = Request.QueryString["collection_status"];
            _newPedido.MedioPago = Request.QueryString["payment_type"];

            if (_newPedido.MercadoPagoStatus.Equals("approved"))
            {
                _newPedido.Id_Estado = 3;
            }

            db.Entry(_newPedido).State = EntityState.Modified;
            db.SaveChanges();

            if (_newPedido.MercadoPagoStatus.Equals("approved"))
            {

                //Envio Mail
                IList<string> _to = new List<string>();
                _to.Add(_newPedido.tbl_Usuarios.Email);
                SendMail(_to, "Tag IT - Muchas Gracias por su compra", TemplatesMails.GetMailConfirmacionCompra(_newPedido));
            }

            NewKeySessionId();

            return RedirectToAction("CustomerOrders");
        }

        public ActionResult CustomerOrders(int? id, int pagina = 1)
        {
            var tbl_Pedidos = db.tbl_Pedidos.Include(t => t.tbl_Estados_Pedido).Include(t => t.tbl_Usuarios);

            if (id != null)
            {
                var _keyUser = KeySessionId();
                tbl_Pedidos = db.tbl_Pedidos.Include(t => t.tbl_Estados_Pedido).Include(t => t.tbl_Usuarios).Where(c => c.Id == id);
            }
            var _r = tbl_Pedidos.Where(u => u.tbl_Usuarios.Email == User.Identity.Name).ToList<tbl_Pedidos>().OrderByDescending(o => o.Fecha);

            Pagination<tbl_Pedidos> _page = new Pagination<tbl_Pedidos>();

            return View(_page.paginado(_r, pagina));
        }

        // GET: tbl_Pedidos/Details/5
        public ActionResult Details(int? id)
        {

            if (!ValidarAdmin())
                return RoutingNoAdmin();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Pedidos tbl_Pedidos = db.tbl_Pedidos.Find(id);
            if (tbl_Pedidos == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Pedidos);
        }

        public ActionResult VerPedido(int? id)
        {         

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Pedidos tbl_Pedidos = db.tbl_Pedidos.Find(id);


            
            if (tbl_Pedidos == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Pedidos);
        }

        // GET: tbl_Pedidos/Create
        public ActionResult Create()
        {
            ViewBag.Id_Estado = new SelectList(db.tbl_Estados_Pedido, "Id", "Estado");
            ViewBag.Id_Usuario = new SelectList(db.tbl_Usuarios, "Id", "Email");
            return View();
        }

        public ActionResult ConfirmPurchase(int? Id_Envios_Parametros, decimal? total)
        {
            int id = 0;
            try
            {
                //crea pedido
                tbl_Pedidos _newPedido = new tbl_Pedidos();
                _newPedido.Fecha = DateTime.Now;
                _newPedido.Id_Estado = 6;
                _newPedido.Id_Envio = Id_Envios_Parametros;
                _newPedido.Total = total;
                _newPedido.keyUser = KeySessionId();
                _newPedido.Id_Usuario = db.tbl_Usuarios.Where(u => u.Email == User.Identity.Name).ToList<tbl_Usuarios>()[0].Id;
                db.tbl_Pedidos.Add(_newPedido);
                db.SaveChanges();

                id = _newPedido.Id;
                NewKeySessionId();

            }
            catch (Exception ex)
            {
                TempData["mensajeWaring"] = "Debe seleccionar un zona de envio.";
                return RedirectToAction("Index", "tbl_Cart");
            }

            return RedirectToAction(string.Format("payOrder/{0}", id));
        }

        public ActionResult payOrder(int? id)
        {
            ViewBag.Id = id;

            return View();
        }
        public async Task<ActionResult> Pago(int? id)
        {
            ViewBag.Id = id;
            string ImageRute = System.Configuration.ConfigurationManager.AppSettings["ImageRuteMp"];

            tbl_Pedidos _pedido = db.tbl_Pedidos.Find(id);

            List<Item> _llItem = new List<Item>();

            foreach (tbl_cart car in _pedido.tbl_cart)
            {
                Item _eItem = new Item();

                _eItem.id = car.tbl_Productos.Codigo;
                _eItem.title = car.tbl_Productos.Nombre;
                _eItem.currency_id = "ARS";
                _eItem.picture_url = string.Format("{0}{1}", ImageRute, car.tbl_Plantillas_Estampas.tbl_Imagenes.URI.Replace("~", "").Replace(" ", "%20"));
                _eItem.description = car.tbl_Productos.Descripcion;
                _eItem.category_id = "Categoria";
                _eItem.Quantity = int.Parse(car.Cantidad.ToString().Split(char.Parse("."))[0]);
                _eItem.UnitPrice = double.Parse(car.PrecioUnitario.ToString());

                _llItem.Add(_eItem);
            }

            //Agrego costo de envio
            Item _eItem2 = new Item();

            _eItem2.id = "CE";
            _eItem2.title = "Costo de envio";
            _eItem2.currency_id = "ARS";
            _eItem2.picture_url = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAOEAAADhCAMAAAAJbSJIAAAArlBMVEX////h9vqa4/BJV3FEU24+TmpzfpGtsrxUYXn09fd2gJI4SWZDUm08TGlHVG6e6fZbd43d3+RFT2t1hJaKx9Z1pLV+h5hNX3iV2+nl+/7J3eTo6u31/P3x8vTo+Pu4ytSmrLedo7BaZ35td4vKzdTBxc3W2d6QmKa1usPi5OiHj5+Um6kyRGOBuclaZn5iboRtl6pliZ56rb7Q5OojOVtXcIdBR2R+tMSKxtZqkaWS90+DAAANBElEQVR4nO1dfZ+auhK2C3HBAFZPq7u1tiq+su7a03vbnvv9v9jNDC4JkEBQEDy/PPvXKpo8ZjLJvGTS6xkYGBgYGBgYGBgYGBgYGBgYGBgYGBgYlMIFtN2JZuCONstocFq9Oc7q1I+Wm9G/iuhoEVrEJ55tBwDb9th/09Ni23bH6sFo51FiW3kEhJLdqO3uXQt3f6LknRIbuhhe8P4Soaf9PYuru3D8ePRs4h+t0+RluXhcLF6i0Dr653G1fWdxtxz3lh+c6a2W61nqvfl6eTqTDIi1b6mH12G7ogH23wv3c+kT803fI/gMfbtDpbOMx8+3lrOCp2bLKT4X+Mub9awezN58VCTksWyOuUyW8dFV0S/ROWxwhnlEb2CWxMPZumm4VzViQVFABwfN52cD/ABdNNmpOvFCcRV4rPCRPQ46fWmsT7ViB/OKONV2KwcHNgb+rqE+1QocQdKXapivMaSf6wNFegcUl0DQj3Kvf/327QPHt295mhGMPe38qrGhMmH7KrJLWGZJonjTjmvUEc7BzAh+ldCLkeEYgaCSTpsbrsNUIhmkXlPzy3McMIrBqssbcRgEe5V6SSafKVlNPb2y8yLQJaxRjYrrfPEA5odxZrNdKl3fuuPaAMvWFzWFDsE0xQ2byIF1647rYslklEyEF8okVCapIOia+9mbYwb7Z09QEzKCz8/PxRRd0Kd2N+2MHesbFax1mYh++f79+5diQd2zyUw6uUGdZfSojOBzn/q0LxtFgeIJZnMXB/GFpLWgbMI9/+VZ3l8yhh/4B9d+R2cim4X2if8r1TIFDIWpeIJF8fYEygBq3uezUL5OFDAU5BS/qnvbUzZ7AosrUimJQoaCnII2DVvgUIg5m4UeNykUK2EhQy6nMKW9rumaDegZ7vSUcyhmyAdxS1MS3w0MmKLxkv9Uu7VihnwmMoH3JrJm2oPrsD5xm0BBoYQhH8SdZwVOt4yoQ0r9KTfcJQyTQQRtSnV9kbfBHrqU6AbljruEYaJrZt1bL5j2C5zkv6sZuquga3vTiSeqBhWDMoYfxO+zB7KGWkPqN1fbvWUMk4nIZELcArYPF7wXiRe/BoaPYKi0w0UON6UZ1KZ9GcNkIoIyPbbDpTd6lGCRspxKGXKoGMKuhixkTTXuTV0fiQTgv6BJ28UM7fA7h4rhCL12MhybdsTB7kwOXYaW7XMoGfqqZrymVWxflvtTjaGIwjGUwu7fgGEgl9LEsihgGJF0f1UMt3IpDW7DULIUz8Hc0dA0H/7jpLusYgi+GprPURnYbTEEJydJDLoiT/fzlxQy7ybrIe5z5a23xJAZT2Tx/p+eL1+GhCG4z6fy1tth2AttHfOwFMk3RJ7UU9MiQ+gR30dez/CU+sVSrbfEEDY1PGShG5HJgltP4DFdyFtviSFod75cXDoRv6a+TrJ5aZHhzBaNi0vFNPk8iIQs/tQiQ5g4wkS8TEy5wzQM5OZhmwxBvR+TiXiZmCZCChsIT+bEuAXDgYLhNh22uGQQ+RDu2c/ly3Jq62f40s9g4LD9ojMQXgnPTj/2esAF65JB5A5hJvLyWH79DPvEziDe3ws4nn9rDOJz87T6IPIhBNNJ7mmrn+FAaSvljKbDkXVLyPaqzJB/FAyQo9Qf3OoYwsZNDBhVlVMuo7DyKBxtrc5DTBcSZauanAoh4B1RJg21qUt7cXCaCst0JYb8Y+DRz2SOJWhzPezFHkBPbP8igr3QU8csWmYYD6LYt0sIQoJqoHJ3t81we4ScNNH3UJ3g3ApSweR8620yjHPSUrFbHXWTyr/EWLIy/bJ1hi4MgL8QXypfNFI5tAtITbSV0d/WGcYJphmvdPEwppO9N0erML20fYa4d8tumouGMZ0EvcUs8YKErw4wRF0fBJnwiTRVP5+sPwIh94pShbrA0J3CnsvORogkHHOHEUYW+NOtohSMLjDszWAgAlmEiJ8pkZ0n6a2P4LO3ClOhOsEwHgqr0rkuAHi52SeLw4PdYNg74FFtycGgIuCRINsriX92hGFvNoVYFJnqn+3dWpgjXiyive4w7M1DGJGARvITzrnHIzwT7Yelj3eGIbPxjhivtUuPAcOhfQuDijoH81q2D1PYYNTUImXlBNwFHq20PK0kr9oYHkYqHELWRii+v1UwmA/iY+iERur5uI1ofCDf72sJdD/bugplohMeqQqwEtjiC/9V9n/jxEfyPTp9WecJzNc7h8Zhfd/RzNKDGLut6loKJV+o4WLL+dokcB/PZSPYQNph9Lgezeau685no/VjdLLi4WOSbGkXxlDnSWQQlJy6UaeUVGKIHOm5U1iTJikDQrz3l32vQuEPbYZlKcZ96isQzxoivKKW0hjrPiOh6gbx+pWySDETZFwOq+y0bR2ahmO2HzCJ5DVpYjHyCLUG+4oJ+Yxh8OtTKf4ARbUdXQzd1SINd7uIHKa+mBCwP5/SozNZlP82OQDDj8OHMgz/Hl9+FFVzxZditt1s9gyb7aVnKTQZPjzgHumyDLhrGF4PXYbDVzaIF2b73wfDs5xeVKDhThg+POFKdknlonthOPzJBtF2yr9S1sZdMHwYfgTf7QUHNe+G4cMTLIoXlKC4H4bDz2NlpLWkjS4xHEqQvPkrF2PQbKNLDF8/5/F0fmD4CdZ9WnV30SmGwx+/Zdvup+TtcYkPXdFGdxg+BFlrBTBOnhhOg+qHUbvFcCw1ysaviZyikVFNTjvPECzIKRfjceUTGp1jSBZxcWL3XKJ4wszt8ef3Z55ATquVnuoew0yABPKNrN+JsgEjI1VwJYPlJAvMGBJf0C4dWAc0GGL2LX+ozBjuEy+D2L8i4HjJ/v1S6DDsOYGgbMqM4QqZezeBFsM1JL2sEmXzikaGSk7vcgx7EyIqm2Jj+B7n4VnZjN+VTWwM6w/EZb62uqDHELNyPK5s0BhW5AJK2+g+Qyxsx5UNGsPaVYvugyFm2CU7m2rG8H0wxMiLoGyqGMN3wnCGYfZE2cTGsF56yJ0w7D2mlQ0aw0ctI+NeGKKy+c2Vjb4xfDcMIU1SUDb6xvDdMMyYUcNPvzWN4fthCNXHBGWjbQx3j2HKAhaNYch15cpG2xjuHEO2TVYB3hSUDUbcCpM739voGsNAgTiaPk0ej42M0tpM3WIo9SZmHG/8cTAyymdipxjKPcIp/P4hPM524OWd7xRDuVc/hVfh4btkKIvMKOI0d8qwEgzDpA3DsEEYhoahYXhuwzBsEIahYWgYntswDBuEYfgvZ5g1B5tjeFjv9/t1I6FhNcPh8On15+eff/55ULOsh+E28ikeqqB+wZm1S6FiOPz0wzq7bH5//POg4FgHw3VI+Qkgj4Z11/yVMxx++jjm+V/BePqzMYaRn85Oseu+sFHKcPh5nD5YFYx/PcmG8WqG8xOeBrV9SFEh8XEuctI7CnwNw79x/PBA3HuzQfCPTJavZDh/w/PY3mBzcHvuYTPBc7KeUydFGcOPmGFJTo9b1tJsvcNTxcFYQvFahiGWS55wHXqYQKjZq7M8fp7h8H+YhbDiM36Ot0kHQoi7JoZLoJMJle/xys4aL6fK+0sx9zBT3WCLhSt+5TXSVQwPSCYbu4pvJa0v8y3HEENmfjbagrU5eHS0HoYQcZVk/mN2Un134mQZYhqJlz+/vsUQd1ZOr2I48xQJK1CJj9R2dVNuDFdQ0Uaiy+DGqPGfYY0MIblDWv0ILzmoWuekqPUUQ8hUl6ZzQSHinNa9iuGANS3PjYPitLXdMJaLro1VErIDzf50KcN+LnI+mypT4/CGMXmkvTJihg9PCaDL8p8PytaP/3lK4UGXoSxynr1ilQOvTlWG2isCN2VTDktZ3A2vDBUfPT+uyVAaM1cc2MT6XapQe1VIotiyst8Mc0ce9C5nONG4wSPPsEEoGcpj/OVLl/wWFqzSJjeV1liDrSGoM2NnSCf/CY1LXOQ36djqJEGopSj7TA3Yr5R37MW1iGu8iAcq/cubGnhW8Hbp15ZhR1Sn7uGHlVwBclVTFpF9IZajr9kO5sAbS6Vyl64RXwOUN9wu1UWBa8DcU9xYikWN671x9y2QHkiFbGTFZqcWRJ48Gw80KanX3bdHKrkp4djN3ig6Ai2evwsSSsXWfs0nZB2Tfpqii5UwG70DDxSAPc1oyBdcveq+jHYEFdrIStzYbFewPTg2e/TLwlqbopjMBziwtRk0CdBlYdPJO6HRBEtE0YZvvT1gYSf/9K7NZkt0t1Us26iHR7xJzKPTaLlYROdKZZVrYFbGFkvhB9SesGZ3J4ouzUYIwuoUOythsxSXhfLIDa5LHcVV+3CPFpcQC45NXc0+C6no9LZpeJurp6OjaA/YvtXgDYLrEH5HMHQ8n9QetlDiEAU+NmuzgVw1PfP3UbhyVmG0v+mNxfPNjjX7dposbnKmFQ/G3x4tNWtgYGBgYGBgYGBgYGBgYGBgYGBgYGDQGv4P4gFMY0tvOHUAAAAASUVORK5CYII=";
            _eItem2.description = _pedido.tbl_Envios_Parametros.Localidad;
            _eItem2.category_id = "Delibery";
            _eItem2.Quantity = 1;
            _eItem2.UnitPrice = double.Parse(_pedido.tbl_Envios_Parametros.Costo_Envio.ToString());

            _llItem.Add(_eItem2);

            string _token = System.Configuration.ConfigurationManager.AppSettings["TokenMp"];

            var Resultado = "";
            var _AccessToken = _token;
            try
            {
                string Url = $"https://api.mercadopago.com/checkout/preferences?access_token={_AccessToken}";
                var Client = new HttpClient();

                var Data = JsonSerializer.Serialize(_llItem);

                Data = "{\"items\":" + Data + ",\"back_urls\":{\"success\":\"" + ImageRute + "tbl_Pedidos/MercadoPagoResponse/" + id + "\",\"pending\": \"" + ImageRute + "Home/Index\",\"failure\": \"" + ImageRute + "tbl_Pedidos/MercadoPagoResponse/" + id + "\"}}";

                HttpContent contenido = new StringContent(Data, Encoding.UTF8, "application/json");


                var httpResponse = await Client.PostAsync(Url, contenido);

                if (httpResponse.IsSuccessStatusCode)
                {
                    Resultado = await httpResponse.Content.ReadAsStringAsync();
                    string[] sResultado = Resultado.Split(char.Parse(","));
                    ViewBag.MpLink = sResultado[17].Replace("\"", "").Replace("init_point:", "");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }


        public ActionResult FinalizarPedido(int? id, string nombreapellidofac, string direccionfac, string pisofac, string dtofac, string cpfac, string nombreapellidoent, string direccionEnt, string pisoent, string dtoent, string cpent, string medioPago)
        {
            tbl_Pedidos _Pedido = db.tbl_Pedidos.Find(id);


            _Pedido.Facturacion_Nombre_Apellido = nombreapellidofac;
            _Pedido.Direccion_Facturacion = direccionfac;
            _Pedido.Facturacion_Piso = pisofac;
            _Pedido.Facturacion_Dto = dtofac;
            _Pedido.Facturacion_CP = cpfac;

            _Pedido.Entrga_Nombre_Apellido = nombreapellidoent;
            _Pedido.Direccion_Entrega = direccionEnt;
            _Pedido.Entrega_Piso = pisoent;
            _Pedido.Entrega_Dto = dtoent;
            _Pedido.Entrega_CP = cpent;

            _Pedido.MedioPago = medioPago;

            db.Entry(_Pedido).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Pago/" + id);

        }

        // POST: tbl_Pedidos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fecha,Id_Usuario,Id_Envio,Id_Pago,Id_Estado,Total")] tbl_Pedidos tbl_Pedidos)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Pedidos.Add(tbl_Pedidos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Estado = new SelectList(db.tbl_Estados_Pedido, "Id", "Estado", tbl_Pedidos.Id_Estado);
            ViewBag.Id_Usuario = new SelectList(db.tbl_Usuarios, "Id", "Email", tbl_Pedidos.Id_Usuario);
            return View(tbl_Pedidos);
        }

        // GET: tbl_Pedidos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Pedidos tbl_Pedidos = db.tbl_Pedidos.Find(id);
            if (tbl_Pedidos == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Estado = new SelectList(db.tbl_Estados_Pedido, "Id", "Estado", tbl_Pedidos.Id_Estado);
            ViewBag.Id_Usuario = new SelectList(db.tbl_Usuarios, "Id", "Email", tbl_Pedidos.Id_Usuario);
            ViewBag.Id_Envio = new SelectList(db.tbl_Envios_Parametros, "Id", "Localidad", tbl_Pedidos.Id_Usuario);
            return View(tbl_Pedidos);
        }

        // POST: tbl_Pedidos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Fecha,Id_Usuario,Id_Envio,Id_Pago,Id_Estado,Total,MedioPago,Direccion_Facturacion,Direccion_Entrega, MercadoPagoId, MercadoPagoStatus, " +
            "Facturacion_Nombre_Apellido, Facturacion_Piso, Facturacion_Dto, Facturacion_CP, Entrga_Nombre_Apellido, Entrega_Piso, Entrega_Dto,Entrega_CP")] tbl_Pedidos tbl_Pedidos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Pedidos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Estado = new SelectList(db.tbl_Estados_Pedido, "Id", "Estado", tbl_Pedidos.Id_Estado);
            ViewBag.Id_Usuario = new SelectList(db.tbl_Usuarios, "Id", "Email", tbl_Pedidos.Id_Usuario);
            return View(tbl_Pedidos);
        }

        // GET: tbl_Pedidos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Pedidos tbl_Pedidos = db.tbl_Pedidos.Find(id);
            if (tbl_Pedidos == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Pedidos);
        }

        // POST: tbl_Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_Pedidos tbl_Pedidos = db.tbl_Pedidos.Find(id);
            db.tbl_Pedidos.Remove(tbl_Pedidos);
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
