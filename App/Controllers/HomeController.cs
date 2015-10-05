using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Handles;
using App.Models;
using App.ViewModels;

namespace App.Controllers
{

    /// <summary>
    /// The default and the only controller used.
    /// </summary>
    public class HomeController : Controller
    {
        LifeHandle LifeHandle;
 
        public HomeController()
        {
            LifeHandle = new LifeHandle();
        }

        /// <summary>
        /// The Default action that loads the initial view file
        /// all adds, updates and deletes
        /// </summary>
        public ActionResult Index()
        {
            return View(new SettingsViewModel());
        }

        /// <summary>
        /// The Default action that loads the initial view file
        /// all adds, updates and deletes
        /// </summary>
        /// <param name="settings">
        /// the viewModel used here for the purpose of setting the grid values on the view
        /// </param>
        /// <returns>Json data for the ajax call from the view</returns>
        public JsonResult GetGridSettings(SettingsViewModel settings)
        {

            settings.interval = LifeHandle.Interval;
            settings.maxHeight = LifeHandle.life.CurrentGeneration.MaxHeight;
            settings.maxWidth = LifeHandle.life.CurrentGeneration.MaxWidth;
            settings.patternTypes = null;
            settings.Cells = null;

            return Json(settings);
        }


        /// <summary>
        /// the main action where all the cell evaluation is done. This function 
        /// accepts the current state of the cells and returns the new state based on the rules specified
        /// </summary>
        /// <param name="settings">
        /// the viewModel used here for the purpose transferring cell data and idenfifing setting change
        /// </param>
        /// <returns>Json data for the ajax call from the view</returns>
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
