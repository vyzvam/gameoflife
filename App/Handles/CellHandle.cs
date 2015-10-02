using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Handles
{
    public class CellHandle
    {
        public CellHandle() {}

        public Cell Clone(Cell cell)
        {
            return new Cell(cell.X, cell.Y, cell.Status);
        }

        public void Kill(Cell cell) { ChangeStatus(cell, LifeState.Dead); }

        public void Enliven(Cell cell) { ChangeStatus(cell, LifeState.Alive); }

        private void ChangeStatus(Cell cell, LifeState state) { cell.Status = (byte)state; }    
    }
}
