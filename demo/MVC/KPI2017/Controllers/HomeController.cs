using KPI2017.Controllers.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KPI2017.Controllers
{
    /// <summary>
    /// Dashboard
    /// </summary>
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}