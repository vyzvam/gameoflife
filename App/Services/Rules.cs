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
    /// <summary>
    /// Class that enforces the rules  applied to the status change of cells
    /// </summary>
    public static class Rules
    {

        /// <summary>
        /// Check if a cell is alive
        /// </summary>
        /// <param name="cell">
        /// the cell object
        /// </param>
        /// <returns>returns true if the cell is alive, false otherwise</returns>
        public static bool IsItAlive(Cell cell)
        {
            return cell.Status.Equals((byte)LifeState.Alive);
        }

        /// <summary>
        /// Check if a cell is dead
        /// </summary>
        /// <param name="cell">
        /// the cell object
        /// </param>
        /// <returns>returns true if the cell is dead, false otherwise</returns>
        public static bool IsItDead(Cell cell)
        {
            return cell.Status.Equals((byte)LifeState.Dead);
        }


        /// <summary>
        /// Check if a cell is undead
        /// </summary>
        /// <param name="cell">
        /// the cell object
        /// </param>
        /// <returns>returns true if the cell is undead, false otherwise</returns>
        public static bool IsItUnDead(Cell cell)
        {
            return cell.Status.Equals((byte)LifeState.Undead);
        }

        /// <summary>
        /// Check if a cell is to be killed
        /// </summary>
        /// <param name="cellGroup">The immediate adjacent cells</param>
        /// <param name="cell">the cell object</param>
        /// <returns>returns true if the cell is to be killed, false otherwise</returns>
        public static bool IsToKill(ConcurrentBag<Cell> cellGroup, Cell cell)
        {
            return !HasTwoOrThreeNeighbours(cellGroup, cell);
        }

        /// <summary>
        /// Check if a cell is to be brought to life
        /// </summary>
        /// <param name="cellGroup">The immediate adjacent cells</param>
        /// <param name="cell">the cell object</param>
        /// <returns>returns true if the cell is to live, false otherwise</returns>
        public static bool IsToGiveLife(ConcurrentBag<Cell> cellGroup, Cell cell)
        {
            return HasThreeNeighbours(cellGroup, cell);
        }

        /// <summary>
        /// Check if a cell has less than two neighbouring living cells 
        /// </summary>
        /// <param name="cellGroup">The immediate adjacent cells</param>
        /// <param name="cell">the cell object</param>
        /// <returns>returns true if so, false otherwise</returns>
        public static bool HasLessThanTwoNeighbours(ConcurrentBag<Cell> cellGroup, Cell cell)
        {

            int totalNeighbours = GetTotalNeighbours(cellGroup, cell);
            return (totalNeighbours < 2) ? true : false;

        }

        /// <summary>
        /// Check if a cell has more than three neighbouring living cells 
        /// </summary>
        /// <param name="cellGroup">The immediate adjacent cells</param>
        /// <param name="cell">the cell object</param>
        /// <returns>returns true if so, false otherwise</returns>
        public static bool HasMoreThanThreeNeighbours(ConcurrentBag<Cell> cellGroup, Cell cell)
        {

            int neighboursCount = GetTotalNeighbours(cellGroup, cell);
            return (neighboursCount > 3) ? true : false;
        }


        /// <summary>
        /// Check if a cell has two or three neighbouring living cells 
        /// </summary>
        /// <param name="cellGroup">The immediate adjacent cells</param>
        /// <param name="cell">the cell object</param>
        /// <returns>returns true if so, false otherwise</returns>
        public static bool HasTwoOrThreeNeighbours(ConcurrentBag<Cell> cellGroup, Cell cell)
        {
            int neighboursCount = GetTotalNeighbours(cellGroup, cell);
            return (neighboursCount.Equals(2) || neighboursCount.Equals(3)) ? true : false;
        }

        /// <summary>
        /// Check if a cell has exactly three neighbouring living cells 
        /// </summary>
        /// <param name="cellGroup">The immediate adjacent cells</param>
        /// <param name="cell">the cell object</param>
        /// <returns>returns true if so, false otherwise</returns>
        public static bool HasThreeNeighbours(ConcurrentBag<Cell> cellGroup, Cell cell)
        {

            int neighboursCount = GetTotalNeighbours(cellGroup, cell);
            return (neighboursCount.Equals(3)) ? true : false;
        }

        /// <summary>
        /// Calculates total number of neighbouring living cells
        /// </summary>
        /// <param name="cellGroup">The immediate adjacent cells</param>
        /// <param name="cell">the cell object</param>
        /// <returns>returns total number of neighbouring living cells</returns>
        private static int GetTotalNeighbours(ConcurrentBag<Cell> cellGroup, Cell cell)
        {
            return cellGroup.Where(m => !m.Equals(cell)).Count(m => m.Status.Equals((byte)LifeState.Alive));

        }

        /// <summary>
        /// Check if the current cell has a undead neighbour cell
        /// </summary>
        /// <param name="cellGroup">The immediate adjacent cells</param>
        /// <param name="cell">the cell object</param>
        /// <returns>returns true if so, false otherwise</returns>
        public static bool HasUndeadNeighbour(ConcurrentBag<Cell> cellGroup, Cell cell)
        {

            int neighboursCount = GetUndeadNeighbours(cellGroup, cell);
            return (neighboursCount.Equals(3)) ? true : false;
        }

        /// <summary>
        /// Calculates total number of neighbouring undead cells
        /// </summary>
        /// <param name="cellGroup">The immediate adjacent cells</param>
        /// <param name="cell">the cell object</param>
        /// <returns>returns total number of neighbouring undead cells</returns>
        private static int GetUndeadNeighbours(ConcurrentBag<Cell> cellGroup, Cell cell)
        {
            return cellGroup.Where(m => !m.Equals(cell)).Count(m => m.Status.Equals((byte)LifeState.Undead));

        }


    }
}
