using App.Models;
using App.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Handles
{

    /// <summary>
    /// Handler class that handles the grid object
    /// </summary>
    public class GridHandle
    {
        private readonly CellHandle cellHandle;
        public GridHandle()
        {
            cellHandle = new CellHandle();
        }


        /// <summary>
        /// processes the grid settings and populates intial cell data
        /// </summary>
        /// <param name="grid">Grid Object to be manipulated</param>
        public void PrepareGrid(Grid grid)
        {

            if (grid.IsGridEmpty)
            {
                SetTinySpace(grid);
            }

            Parallel.For(0, grid.MaxHeight, y =>
            {
                Parallel.For(0, grid.MaxWidth, x =>
                {
                    grid.Cells.Add(new Cell(x, y));

                });
            });

        }

        /// <summary>
        /// Sets tiny width and height of a grid dynamically
        /// </summary>
        /// <param name="grid">Grid Object to be manipulated</param>
        public void SetTinySpace(Grid grid)
        {
            grid.MaxWidth = int.MaxValue / 10000000;
            grid.MaxHeight = int.MaxValue / 10000000;
        }

        /// <summary>
        /// Sets small width and height of a grid dynamically
        /// </summary>
        /// <param name="grid">Grid Object to be manipulated</param>
        public void SetSmallSpace(Grid grid)
        {
            grid.MaxWidth = int.MaxValue / 1000000;
            grid.MaxHeight = int.MaxValue / 1000000;
        }

        /// <summary>
        /// Sets medium width and height of a grid dynamically
        /// </summary>
        /// <param name="grid">Grid Object to be manipulated
        public void SetMediumSpace(Grid grid)
        {
            grid.MaxWidth = int.MaxValue / 100000;
            grid.MaxHeight = int.MaxValue / 100000;
        }

        /// <summary>
        /// Sets large width and height of a grid dynamically
        /// </summary>
        /// <param name="grid">Grid Object to be manipulated
        public void SetLargeSpace(Grid grid)
        {
            grid.MaxWidth = int.MaxValue / 10000;
            grid.MaxHeight = int.MaxValue / 10000;
        }

        /// <summary>
        /// Sets huge width and height of a grid dynamically
        /// </summary>
        /// <param name="grid">Grid Object to be manipulated
        public void SetHugeSpace(Grid grid)
        {
            grid.MaxWidth = int.MaxValue / 1000;
            grid.MaxHeight = int.MaxValue / 1000;
        }

        /// <summary>
        /// Creates a collection of cells based on the Glider pattern 
        /// </summary>
        /// <returns>returns a collection of cells</returns>
        public ICollection<Cell> GetGlider()
        {
            byte status = (byte)LifeState.Alive;
            return new List<Cell>() {
                new Cell(5, 5, status),
                new Cell (6, 6, status),
                new Cell(4, 7, status),
                new Cell(5, 7, status),
                new Cell(6, 7, status)
            };
        }

        /// <summary>
        /// Creates 2 collections of cells based on the Glider pattern 
        /// </summary>
        /// <returns>returns a collection of cells</returns>
        public ICollection<Cell> GetDoubleGlider()
        {
            byte status = (byte)LifeState.Alive;
            return new List<Cell>() {
                new Cell(4, 5, status),
                new Cell (5, 6, status),
                new Cell(3, 7, status),
                new Cell(4, 7, status),
                new Cell(5, 7, status),

                new Cell(9, 5, status),
                new Cell (10, 6, status),
                new Cell(8, 7, status),
                new Cell(9, 7, status),
                new Cell(10, 7, status)
            };
        }

        /// <summary>
        /// Creates a collection of cells based on the Acorn pattern 
        /// </summary>
        /// <returns>returns a collection of cells</returns>
        public ICollection<Cell> GetAcorn()
        {
            byte status = (byte)LifeState.Alive;
            return new List<Cell>() {
                new Cell(5, 5, status),
                new Cell (7, 6, status),
                new Cell(4, 7, status),
                new Cell(5, 7, status),
                new Cell(8, 7, status),
                new Cell(9, 7, status),
                new Cell(10, 7, status),
            };
        }


        /// <summary>
        /// changes the status of cells of a grid based on a set of cells provided
        /// </summary>
        /// <param name="grid">Grid Object to be manipulated</param>
        /// <param name="cells">collection of cells to be used for validation</param>
        public void Populate(Grid grid, IEnumerable<Cell> cells)
        {
            Parallel.ForEach(cells, (item) =>
            {
                var targetCell = grid.Cells.FirstOrDefault(m => m.Equals(item));
                if (targetCell != null)
                {

                    if (Rules.IsItAlive(item)) { cellHandle.Enliven(targetCell); }
                    else if (Rules.IsItDead(item)) { cellHandle.Kill(targetCell); }
                    else if (Rules.IsItUnDead(item)) { cellHandle.Resurrect(targetCell); }

                }
            });

        }



        /// <summary>
        /// creates a new collection of cells (cloned)
        /// </summary>
        /// <param name="grid">Grid Object to be validated against</param>
        /// <returns>enumerations of cells after validation</returns>
        public IEnumerable<Cell> GetCellsCopy(Grid grid)
        {
            ConcurrentBag<Cell> cellsToShow = new ConcurrentBag<Cell>();

            Parallel.ForEach(grid.Cells, (cell) => { cellsToShow.Add(cellHandle.Clone(cell)); });

            //sort not required
            return cellsToShow.OrderBy(m => m.X).OrderBy(m => m.Y).AsEnumerable();

        }

        /// <summary>
        /// replaces collection of cells supplied to the function
        /// </summary>
        /// <param name="targetGrid">Grid Object to be manipulated</param>
        /// <param name="sourceGrid">Grid Object to be validated against</param>
        public void CopyGenerationCells(Grid targetGrid, Grid sourceGrid)
        {
            targetGrid.Cells = new ConcurrentBag<Cell>();
            Parallel.ForEach(sourceGrid.Cells, (cell) => { targetGrid.Cells.Add(cellHandle.Clone(cell)); });

        }

        /// <summary>
        /// replaces collection of cells supplied to the function
        /// </summary>
        /// <param name="cells">all cells from a grid</param>
        /// <param name="cell">selected cell</param>
        /// <returns>returns a thread-safe collection of cells (adjacent cells)</returns>
        private ConcurrentBag<Cell> GetCellGroup(ConcurrentBag<Cell> cells, Cell cell)
        {

            ConcurrentBag<Cell> groupCells = new ConcurrentBag<Cell>();

            Parallel.For(-1, 2, y =>
            {
                Parallel.For(-1, 2, x =>
                {
                    var currentCell = cells.FirstOrDefault(m => m.X.Equals(cell.X + x) && m.Y.Equals(cell.Y + y));
                    if (currentCell != null) { groupCells.Add(currentCell); }

                });
            });

            return groupCells;
        }

        /// <summary>
        /// Changes the current status of a cell based on a set of applied rules
        /// </summary>
        /// <param name="targetGrid">Grid Object to be manipulated</param>
        /// <param name="sourceGrid">Grid Object to be validated against</param>
        /// <param name="isToZombify">a paramenter that determines a change in the rule</param>
        public void DecideCellFate(Grid targetGrid, Grid sourceGrid, Cell sourceCell, bool isToZombify = false)
        {
            var sourceCellGroup = GetCellGroup(sourceGrid.Cells, sourceCell);

            Cell targetCell = targetGrid.Cells.FirstOrDefault(m => m.Equals(sourceCell));

            if (Rules.IsItAlive(sourceCell))
            {
                if (Rules.IsToKill(sourceCellGroup, sourceCell))
                {
                    cellHandle.Kill(targetCell);
                }

            }
            else if (Rules.IsToGiveLife(sourceCellGroup, sourceCell))
            {
                cellHandle.Enliven(targetCell);
            }

            if (isToZombify)
            {
                if (Rules.IsItAlive(sourceCell))
                {
                    if (Rules.HasUndeadNeighbour(sourceCellGroup, sourceCell))
                    {
                        cellHandle.Kill(targetCell);
                    }

                }

                else if (Rules.IsItDead(sourceCell))
                {
                    cellHandle.Resurrect(targetCell);
                }
            }


        }
    }
}
