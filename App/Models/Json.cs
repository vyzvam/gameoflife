using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Models
{
    /// <summary>
    /// ViewModel that is used for data exchange with the view
    /// </summary>
    public class LifeSettings
    {
        public int interval { get; set; }
        public int maxWidth { get; set; }
        public int maxHeight { get; set; }
    }

}