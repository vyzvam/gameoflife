using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Handles
{

    public class GridHandle
    {

        private readonly CellHandle cellHandle;
        public GridHandle() {
            cellHandle = new CellHandle();
        }

    }
}
