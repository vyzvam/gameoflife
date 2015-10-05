using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    /// <summary>
    /// An enumerator to easily track status of cells
    /// </summary>
    public enum LifeState
    {
        Unknown = 0,
        Alive = 1,
        Dead = 2,
        Undead = 3
    }

    /// <summary>
    /// Model / Container class that holds details about a cell
    /// </summary>
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public byte Status { get; set; }

        public Cell() { }

        public Cell(byte status)
        {
            Status = status;
        }

        public Cell(int x, int y, byte status = (byte)LifeState.Unknown)
            : this(status)
        {
            X = x;
            Y = y;
        }


        public static bool operator ==(Cell a, Cell b)
        {
            if (object.ReferenceEquals(a, null))
            {
                if (object.ReferenceEquals(b, null)) { return true; }
                else { return false; }
            }
            else if (object.ReferenceEquals(b, null))
            {
                if (object.ReferenceEquals(a, null)) { return true; }
                else { return false; }
            }
            else
            {
                return a.X == b.X && a.Y == b.Y;
            }
        }

        public static bool operator !=(Cell a, Cell b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }

            if (!(obj is Cell)) { return false; }

            Cell p = (Cell)obj;

            return p == this;
        }

        public bool Equals(Cell a)
        {
            return this == a;
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

    }
}
