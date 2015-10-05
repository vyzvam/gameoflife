using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Handles
{
    /// <summary>
    /// The class that manages the cell object
    /// </summary>
    public class CellHandle
    {
        public CellHandle() {}

        /// <summary>
        /// Replicates a cell (so the the original cell value is not disturbed) 
        /// </summary>
        /// <param name="cell">the cell object</param>
        /// <returns>returns a cell object cloned from the supplied cell object</returns>
        public Cell Clone(Cell cell)
        {
            return new Cell(cell.X, cell.Y, cell.Status);
        }

        /// <summary>
        /// Helper function that changes the status of a cell to Dead
        /// </summary>
        /// <param name="cell">the cell object</param>
        public void Kill(Cell cell) { ChangeStatus(cell, LifeState.Dead); }

        /// <summary>
        /// Helper function that changes the status of a cell to Alive
        /// </summary>
        /// <param name="cell">the cell object</param>
        public void Enliven(Cell cell) { ChangeStatus(cell, LifeState.Alive); }

        /// <summary>
        /// Helper function that changes the status of a cell to Undead
        /// </summary>
        /// <param name="cell">the cell object</param>
        public void Resurrect(Cell cell) { ChangeStatus(cell, LifeState.Undead); }

        /// <summary>
        /// Changes the status of cell
        /// </summary>
        /// <param name="cell">the cell object</param>
        /// <param name="cell">the status enum</param>
        private void ChangeStatus(Cell cell, LifeState state) { cell.Status = (byte)state; }    
    }
}
