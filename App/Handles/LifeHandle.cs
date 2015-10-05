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

        public LifeHandle(int maxGen = int.MaxValue, int interval = 50, int gridXSize = 20, int gridYSize = 20)
        {
            life = new Life(gridXSize, gridYSize);
            Interval = interval;
            MaxGenerations = maxGen;
            GridHandle = new GridHandle();
        }

        public void StartLife()
        {
            PrepareEnvironment();
        }

        public void StartPopulation(int patternType = 0, IEnumerable<Cell> cells = null)
        {
            if (cells == null)
            {
                 if (patternType.Equals(0)) { cells = GridHandle.GetGlider(); }
                else if (patternType.Equals(1)) { cells = GridHandle.GetDoubleGlider(); }
                else { cells = GridHandle.GetAcorn(); }
            }

            GridHandle.Populate(life.CurrentGeneration, cells);
            GridHandle.Populate(life.ProxyGeneration, cells);
        }

        public void PrepareEnvironment()
        {
            GridHandle.PrepareGrid(life.CurrentGeneration);
            GridHandle.PrepareGrid(life.ProxyGeneration);
        }

        public void StartLifeCycle()
        {
            PrepareEnvironment();
            StartPopulation();
            Evolve();
        }

        public void Evolve()
        {
            ShowCurrentGeneration();
            for (GenerationCounter = 1; GenerationCounter < MaxGenerations; GenerationCounter++)
            {
                AdvanceGeneration();
                ShowCurrentGeneration();
                System.Threading.Thread.Sleep(Interval);
            }
        }




        public Grid getCurrentGeneration() {

            return life.CurrentGeneration;
        }

        public IEnumerable<Cell> GetCurrentGenerationCells()
        {
            return GridHandle.GetCellsCopy(life.CurrentGeneration);
        }

        public void ShowCurrentGeneration()
        {
            Console.Clear();
            //Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Current Generation: {0}, Total Living Cells : {1}", GenerationCounter, life.CurrentGeneration.Cells.Count(m => m.Status.Equals((byte)LifeState.Alive)));
            var grid = GetCurrentGenerationCells();

            foreach (var cell in grid)
            {
                if (cell.X.Equals(0)) { Console.WriteLine(Environment.NewLine); }

                Console.Write((cell.Status.Equals((byte)LifeState.Alive)) ? "0" : " ");

            }

        }

        public void AdvanceGeneration()
        {
            Parallel.ForEach(life.CurrentGeneration.Cells, (cell) =>
            {

                GridHandle.DecideCellFate(life.ProxyGeneration, life.CurrentGeneration, cell);
            });

            GridHandle.CopyGenerationCells(life.CurrentGeneration, life.ProxyGeneration);
        }


    }
}
