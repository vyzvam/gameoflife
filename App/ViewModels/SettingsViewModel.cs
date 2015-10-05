using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.ViewModels
{
    public class SettingsViewModel
    {
        public int interval { get; set; }
        public int maxWidth { get; set; }
        public int maxHeight { get; set; }
        public int patternTypeId { get; set; }
        public Dictionary<int, string> patternTypes { get; set; }
        public bool IsToZombify { get; set; }

        public string Cells { get; set; }

        public SettingsViewModel()
        {
            patternTypes = new Dictionary<int, string>()
            {
                {0, "Glider"},
                {1, "2 Gliders"},
                {2, "Acorn"},
            };
        }
    }
}