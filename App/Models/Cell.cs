using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public enum LifeState
    {
        Unknown = 0,
        Alive = 1,
        Dead = 2
    }

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

        public Cell (int x, int y, byte status = (byte)LifeState.Unknown) : this(status)
        {
            X = x;
            Y = y;
        }

    }
}
