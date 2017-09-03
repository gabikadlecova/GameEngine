using Casting.Environment.Interfaces;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Casting.Environment.Tools
{
    public class MapReader : IMapReader
    {

        public IMap ReadMap(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            //todo check
            int[,] board = new int[lines.Length, lines[0].Length / 2 + 1];

            for (int y = 0; y < lines.Length; y++)
            {
                string line = lines[y];
                int[] numbers = line.Split(',').Select(c => Convert.ToInt32(c)).ToArray();
                for (int x = 0; x < numbers.Length; x++)
                {
                    board[y, x] = numbers[x];
                }
            }

            return new Map(board);
        }

        public IContainer<IWall> ReadWalls(string filePath)
        {
            IContainer<IWall> container = new Container<IWall>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string[] wallData = reader.ReadLine()?.Split(',');

                    if (wallData != null)
                    {
                        try
                        {
                            int id = Int32.Parse(wallData[0]);
                            string textureX = wallData[1];
                            string textureY = wallData[2];
                            uint altX = uint.Parse(wallData[3], NumberStyles.HexNumber);
                            uint altY = uint.Parse(wallData[4], NumberStyles.HexNumber);
                            
                            int maxHeight = Int32.Parse(wallData[5]);
                            //todo color!!
                            IWall wall = new Wall(textureX, textureY, new Color(){PackedValue = altX}, new Color() {PackedValue = altY}, maxHeight  );

                            container[id] = wall;

                            /*wall.TextureX.LoadBitmap();
                            wall.TextureY.LoadBitmap();*/
                        }
                        catch (InvalidCastException e)
                        {
                            Debug.WriteLine(e);
                            //todo
                            //throw;
                        }
 
                    }
                }

                return container;
            }
        }
    }
}
