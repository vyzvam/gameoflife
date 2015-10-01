using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{

    //TODO: Planning
    //TODO: Read about Conway's game of life
    //TODO: Use .Net MVC & Jquery / javascript
    //TODO: ajax call to make and get requests
    //TODO: Create model classes, possibly (life, grid, cell)
    //TODO: Create Handlers for the models
    //TODO: Create rule class
    //TODO: processing data in parallel
    //TODO: include jquery
    //TODO: javascript, create literal to contain settings, make ajax calls and draw canvas

    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
