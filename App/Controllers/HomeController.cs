using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{

    //TODO: processing data in parallel
    //TODO: include jquery
    //TODO: javascript, create literal to contain settings, make ajax calls and draw canvas

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetGridSettings()
        {

        }


        public JsonResult GetGenerationData(string cells)
        {
        }

    }
}
