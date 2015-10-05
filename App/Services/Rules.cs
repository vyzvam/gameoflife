using App.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Services
{
    public static class Rules
    {
        public static bool IsItAlive(Cell cell)
        {
            return cell.Status.Equals((byte)LifeState.Alive);
        }

        public static bool IsItDead(Cell cell)
        {
            return cell.Status.Equals((byte)LifeState.Dead);
        }

        public static bool IsItUnDead(Cell cell)
        {
            return cell.Status.Equals((byte)LifeState.Undead);
        }

        public static bool IsToKill(ConcurrentBag<Cell> cellGroup, Cell cell)
        {
            return !HasTwoOrThreeNeighbours(cellGroup, cell);
        }

        public static bool IsToGiveLife(ConcurrentBag<Cell> cellGroup, Cell cell)
        {
            return HasThreeNeighbours(cellGroup, cell);
        }

        public static bool HasLessThanTwoNeighbours(ConcurrentBag<Cell> cellGroup, Cell cell)
        {

            int totalNeighbours = GetTotalNeighbours(cellGroup, cell);
            return (totalNeighbours < 2) ? true : false;

        }

        public static bool HasMoreThanThreeNeighbours(ConcurrentBag<Cell> cellGroup, Cell cell)
        {

            int neighboursCount = GetTotalNeighbours(cellGroup, cell);
            return (neighboursCount > 3) ? true : false;
        }

        public static bool HasTwoOrThreeNeighbours(ConcurrentBag<Cell> cellGroup, Cell cell)
        {
            int neighboursCount = GetTotalNeighbours(cellGroup, cell);
            return (neighboursCount.Equals(2) || neighboursCount.Equals(3)) ? true : false;
        }

        public static bool HasThreeNeighbours(ConcurrentBag<Cell> cellGroup, Cell cell)
        {

            int neighboursCount = GetTotalNeighbours(cellGroup, cell);
            return (neighboursCount.Equals(3)) ? true : false;
        }

        private static int GetTotalNeighbours(ConcurrentBag<Cell> cellGroup, Cell cell)
        {
            return cellGroup.Where(m => !m.Equals(cell)).Count(m => m.Status.Equals((byte)LifeState.Alive));

        }

        public static bool HasUndeadNeighbour(ConcurrentBag<Cell> cellGroup, Cell cell)
        {

            int neighboursCount = GetUndeadNeighbours(cellGroup, cell);
            return (neighboursCount.Equals(3)) ? true : false;
        }

        private static int GetUndeadNeighbours(ConcurrentBag<Cell> cellGroup, Cell cell)
        {
            return cellGroup.Where(m => !m.Equals(cell)).Count(m => m.Status.Equals((byte)LifeState.Undead));

        }


    }
}
