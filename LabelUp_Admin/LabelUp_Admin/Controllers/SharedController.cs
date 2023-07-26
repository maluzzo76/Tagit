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
    public class SharedController : CustomController
    {
        public LabelUpV2Entities1 db = new LabelUpV2Entities1();
        // GET: Shared
        public ActionResult _HomeStore()
        {
            return View(db.tbl_Productos.Where(e=>e.Is_Destacado==true).ToList<tbl_Productos>());
        }
    }
}