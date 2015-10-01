using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Life
    {
        public Grid CurrentGeneration { get; set; }
        public Grid ProxyGeneration { get; set; }

        public Life(int gridXSize = 20, int gridYSize = 20)
        {
            CurrentGeneration = new Grid(gridXSize, gridYSize);
            ProxyGeneration = new Grid(gridXSize, gridYSize);

        }
    }
}
