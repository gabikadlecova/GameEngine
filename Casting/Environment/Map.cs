using System;
using System.IO;
using System.Linq;
using Casting.Environment.Interfaces;

namespace Casting.Environment
{
    /// <summary>
    /// Provides access to game map data
    /// </summary>
    public class Map : IMap
    {
        /// <summary>
        /// Array of map tile values
        /// </summary>
        private readonly int[,] _board;
        
        /// <summary>
        /// Map width
        /// </summary>
        public int Width { get { return _board.GetLength(0); } }

        /// <summary>
        /// Map height
        /// </summary>
        public int Height { get { return _board.GetLength(1); } }

        /// <summary>
        /// Initializes a map object which holds an array of map tiles
        /// </summary>
        /// <param name="board">Game map</param>
        public Map(int[,] board)
        {
            _board = board;
        }

        
        /// <summary>
        /// Determines whether the specified point in space lies within the map or not
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>True for all point that lie within the map</returns>
        public bool IsInRange(int x, int y)
        {
            if (_board.GetUpperBound(0) < x || x < 0)
                return false;

            return _board.GetUpperBound(1) >= y && y >= 0;
        }

        /// <summary>
        /// Provides access to map tiles
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>Map tile which has these coordinates</returns>
        public int this[int x, int y]
        {
            get { return _board[x, y]; }
        }
    }
}
