using App.Handles;
using App.Models;
using App.ViewModels;
using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        LifeHandle LifeHandle;
 
        public HomeController()
        {
            LifeHandle = new LifeHandle();
        }
        
        public ActionResult Index()
        {
            return View(new SettingsViewModel());
        }

        public JsonResult GetGridSettings(SettingsViewModel settings)
        {

            settings.interval = LifeHandle.Interval;
            settings.maxHeight = LifeHandle.life.CurrentGeneration.MaxHeight;
            settings.maxWidth = LifeHandle.life.CurrentGeneration.MaxWidth;
            settings.patternTypes = null;
            settings.Cells = null;

            return Json(settings);
        }


        public JsonResult GetGenerationData(SettingsViewModel settings)
        {
            LifeHandle.StartLife();

            if (!string.IsNullOrEmpty(settings.Cells) && !settings.Cells.Equals("null"))
            {
                string _cells = settings.Cells.Replace(@"\", string.Empty);
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var cells = serializer.Deserialize<IEnumerable<Cell>>(_cells);

                LifeHandle.StartPopulation(0, cells);
                LifeHandle.AdvanceGeneration(settings.IsToZombify);
            }
            else
            {
                LifeHandle.StartPopulation(settings.patternTypeId);
            }

            var grid = LifeHandle.getCurrentGeneration();
            //var jsonCells = grid.Cells.Where(m => m.Status.Equals((byte)LifeState.Alive));
            var jsonCells = grid.Cells;

            return Json(jsonCells);
        }
    }
}
