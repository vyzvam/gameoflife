using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Handles
{
    public class LifeHandle
    {
        public int GenerationCounter { get; set; }
        public int MaxGenerations { get; set; }
        public int Interval { get; set; }

        public Life life { get; set; }

        private readonly GridHandle GridHandle;


    }
}
