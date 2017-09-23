using Casting.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Casting.Player;
using Casting.Player.Interfaces;
using Casting.RayCasting;
using Casting.RayCasting.Interfaces;
using Input;
using Microsoft.Xna.Framework;

namespace Casting.Environment.Tools
{
    public class GameReader : IMapReader
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
                            IWall wall = new Wall(textureX, textureY, new Color() { PackedValue = altX },
                                new Color() { PackedValue = altY }, maxHeight);

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

        public Dictionary<int, EnemyData> ReadEnemies(string filePath)
        {
            Dictionary<int, EnemyData> enemies = new Dictionary<int, EnemyData>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string[] data = reader.ReadLine()?.Split(',');

                    if (data != null)
                    {
                        try
                        {
                            int typeId = Int32.Parse(data[0]);
                            int hitPoints = Int32.Parse(data[1]);
                            float positionX = float.Parse(data[2], CultureInfo.InvariantCulture);
                            float positionY = float.Parse(data[3], CultureInfo.InvariantCulture);
                            string texturePath = data[4];
                            string killedTexture = data[5];
                            int height = Int32.Parse(data[6]);
                            int width = Int32.Parse(data[7]);
                            float speed = float.Parse(data[8], CultureInfo.InvariantCulture);
                            float hitBox = float.Parse(data[9], CultureInfo.InvariantCulture);
                            float spawnTime = float.Parse(data[10], CultureInfo.InvariantCulture);

                            SpriteData spriteData = new SpriteData(texturePath, killedTexture, height, width);
                            EnemyData enemyData = new EnemyData(typeId, spriteData, hitPoints, hitBox, speed, spawnTime);

                            enemies.Add(typeId, enemyData);
                        }
                        catch (InvalidCastException e)
                        {
                            Debug.WriteLine(e);
                            //todo what error
                            //throw;
                        }

                    }

                }

            }

            return enemies;
        }

        public List<IWeapon> ReadWeapons(string filePath)
        {
            List<IWeapon> weapons = new List<IWeapon>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine()?.Split(',');

                    if (line != null)
                    {
                        try
                        {
                            int maxAmmo = Int32.Parse(line[0]);
                            string picAddress = line[1];
                            float shootingSpeed = float.Parse(line[2], CultureInfo.InvariantCulture);
                            string flyPicAdd = line[3];
                            string hitPicAdd = line[4];
                            int bulletSize = Int32.Parse(line[5]);

                            SpriteData bullet = new SpriteData(flyPicAdd, hitPicAdd, bulletSize, bulletSize);
                            IWeapon weapon = new BasicWeapon(maxAmmo, picAddress, bullet, shootingSpeed);
                            weapons.Add(weapon);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e);
                            throw;
                        }
                    }

                }
            }

            return weapons;
        }

        public EngineSettings LoadSettings(string path)
        {
            try
            {
                string[] lines = File.ReadAllLines(path);
                string wall = lines[0];
                string map = lines[1];
                string enemies = lines[2];
                string weapons = lines[3];
                string player = lines[4];
                string sky = lines[5];
                string floor = lines[6];
                ICastCondition condition = CastCondition.WallCountInterval(4, 1);

                return new EngineSettings(wall, map, enemies, weapons, player, sky, floor, condition);


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
