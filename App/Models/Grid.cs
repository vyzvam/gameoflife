using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Grid
    {
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }
        public ConcurrentBag<Cell> Cells { get; set; }

        public bool IsGridEmpty 
        {
            get { return (MaxHeight.Equals(0) || MaxWidth.Equals(0)) ? true : false; }
        }

        public Grid(int maxWidth = 0, int maxHeight = 0)
        {
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;
            Cells = new ConcurrentBag<Cell>();
        }
    }
}
