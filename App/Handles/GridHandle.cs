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

    public class GridHandle
    {
        private readonly CellHandle cellHandle;
        public GridHandle()
        {
            cellHandle = new CellHandle();
        }

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

        public void SetTinySpace(Grid grid)
        {
            grid.MaxWidth = int.MaxValue / 10000000;
            grid.MaxHeight = int.MaxValue / 10000000;
        }

        public void SetSmallSpace(Grid grid)
        {
            grid.MaxWidth = int.MaxValue / 1000000;
            grid.MaxHeight = int.MaxValue / 1000000;
        }

        public void SetMediumSpace(Grid grid)
        {
            grid.MaxWidth = int.MaxValue / 100000;
            grid.MaxHeight = int.MaxValue / 100000;
        }

        public void SetLargeSpace(Grid grid)
        {
            grid.MaxWidth = int.MaxValue / 10000;
            grid.MaxHeight = int.MaxValue / 10000;
        }

        public void SetHugeSpace(Grid grid)
        {
            grid.MaxWidth = int.MaxValue / 1000;
            grid.MaxHeight = int.MaxValue / 1000;
        }

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



        public IEnumerable<Cell> GetCellsCopy(Grid grid)
        {
            ConcurrentBag<Cell> cellsToShow = new ConcurrentBag<Cell>();

            Parallel.ForEach(grid.Cells, (cell) => { cellsToShow.Add(cellHandle.Clone(cell)); });

            //sort not required
            return cellsToShow.OrderBy(m => m.X).OrderBy(m => m.Y).AsEnumerable();

        }

        public void CopyGenerationCells(Grid targetGrid, Grid sourceGrid)
        {
            targetGrid.Cells = new ConcurrentBag<Cell>();
            Parallel.ForEach(sourceGrid.Cells, (cell) => { targetGrid.Cells.Add(cellHandle.Clone(cell)); });

        }

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

        public void DecideCellFate(Grid targetGrid, Grid SourceGrid, Cell sourceCell, bool isToZombify = false)
        {
            var sourceCellGroup = GetCellGroup(SourceGrid.Cells, sourceCell);

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
