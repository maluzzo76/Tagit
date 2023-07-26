using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace LabelUp_Admin.Controllers
{
    public class LoginToChartController : Controller
    {
        // GET: LoginToChart
        public ActionResult login() 
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "tbl_Cart");
            }

            return View();
        }
    }
}