using System;
using System.IO;
using System.Linq;
using Casting.Environment.Interfaces;

namespace Casting.Environment
{
    public class Map : IMap
    {
        private readonly int[,] _board;
        
        public int Width { get { return _board.GetLength(0); } }

        public int Height { get { return _board.GetLength(1); } }

        public Map(int[,] board)
        {
            _board = board;
        }

        public bool IsInRange(int x, int y)
        {
            if (_board.GetUpperBound(0) < x || x < 0)
                return false;

            return _board.GetUpperBound(1) >= y && y >= 0;
        }

        public int this[int x, int y]
        {
            get { return _board[x, y]; }
        }
    }
}
