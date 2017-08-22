using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KPI2017.Controllers.common
{
    public class BaseController : Controller
    {
        public BaseController()
        {
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);


        }

    }
}