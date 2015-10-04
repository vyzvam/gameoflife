using App.Handles;
using App.Models;
using MvcApplication1.Models;
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
        LifeHandle LifeHandle;
        public HomeController()
        {
            LifeHandle = new LifeHandle();
        }
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetGridSettings()
        {

            var settings = new LifeSettings
            {
                interval = LifeHandle.Interval,
                maxHeight = LifeHandle.life.CurrentGeneration.MaxHeight,
                maxWidth = LifeHandle.life.CurrentGeneration.MaxWidth
            };


            return Json(settings);
        }


        public JsonResult GetGenerationData(string cells)
        {
            LifeHandle.StartLife();

            if (!string.IsNullOrEmpty(cells) && !cells.Equals("null"))
            {
                string _cells = cells.Replace(@"\", string.Empty);
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var liveCells = serializer.Deserialize<IEnumerable<Cell>>(_cells);

                LifeHandle.StartPopulation(liveCells);
                LifeHandle.AdvanceGeneration();
            }
            else
            {
                LifeHandle.StartPopulation();
            }

            var grid = LifeHandle.getCurrentGeneration();
            var jsonCells = grid.Cells.Where(m => m.Status.Equals((byte)LifeState.Alive));

            return Json(jsonCells);
        }
    }
}
