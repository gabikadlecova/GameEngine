using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Casting.Environment;
using Casting.Environment.Interfaces;
using Casting.Player;
using Casting.Player.Interfaces;
using Casting.RayCasting;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;

namespace Input
{
    /// <summary>
    /// Reads game data from specified input text files
    /// </summary>
    public class GameReader
    {
        /// <summary>
        /// Reads the game map
        /// </summary>
        /// <param name="filePath">Game map file path</param>
        /// <returns>IMap game map object</returns>
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

        /// <summary>
        /// Reads all possible wall types
        /// </summary>
        /// <param name="filePath">Wall type path</param>
        /// <returns>IContainer of all wall types</returns>
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

        /// <summary>
        /// Reads all enemy type data
        /// </summary>
        /// <param name="filePath">Enemy data path</param>
        /// <param name="caster">Default movement raycaster</param>
        /// <returns>Dictionary of all possible enemy types</returns>
        public Dictionary<int, EnemyData> ReadEnemies(string filePath, IRayCaster caster)
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
                            //could be used as initial position
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
                            EnemyData enemyData = new EnemyData(typeId, spriteData, hitPoints, hitBox, speed, spawnTime, caster);

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

        /// <summary>
        /// Reads all available weapons
        /// </summary>
        /// <param name="filePath">Weapon file path</param>
        /// <param name="caster">Default shooting raycaster</param>
        /// <returns>List of read weapons</returns>
        public List<IWeapon> ReadWeapons(string filePath, IRayCaster caster)
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
                            float minBulletDist = float.Parse(line[6], CultureInfo.InvariantCulture);

                            SpriteData bullet = new SpriteData(flyPicAdd, hitPicAdd, bulletSize, bulletSize);
                            IWeapon weapon = new BasicWeapon(maxAmmo, picAddress, bullet, shootingSpeed, minBulletDist, caster);
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

        /// <summary>
        /// Reads player data
        /// </summary>
        /// <param name="filePath">Player data file path</param>
        /// <param name="caster">Default movement raycaster</param>
        /// <returns>Player data</returns>
        public Player ReadPlayer(string filePath, IRayCaster caster)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {

                string[] line = reader.ReadLine()?.Split(',');

                if (line != null)
                {
                    try
                    {
                        float posX = float.Parse(line[0], CultureInfo.InvariantCulture);
                        float posY = float.Parse(line[1], CultureInfo.InvariantCulture);
                        float dirX = float.Parse(line[2], CultureInfo.InvariantCulture);
                        float dirY = float.Parse(line[3], CultureInfo.InvariantCulture);
                        int hitPoints = Int32.Parse(line[4]);
                        string name = line[5];
                        float speed = float.Parse(line[6], CultureInfo.InvariantCulture);

                        return new Player(posX, posY, dirX, dirY, hitPoints, HumanCastCondition.Default(), name, speed, caster);

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        throw;
                    }
                }

                throw new Exception("Could not load player");

            }
        }

        /// <summary>
        /// Loads game settings from a text file
        /// </summary>
        /// <param name="path">Settings file path</param>
        /// <returns>Game settings</returns>
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
                int enemySecs = Int32.Parse(lines[7]);
                int width = Int32.Parse(lines[8]);
                int height = Int32.Parse(lines[9]);
                ICastCondition condition = CastCondition.LimitWalls(4);

                return new EngineSettings(wall, map, enemies, weapons, player, sky, floor, condition, enemySecs, width, height);


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
