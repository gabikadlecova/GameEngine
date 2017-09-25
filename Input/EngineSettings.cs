using Casting.Player.Interfaces;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;

namespace Input
{
    /// <summary>
    /// This class contains basic game settings which are necessary for the game.
    /// </summary>
    public class EngineSettings
    {
        /// <summary>
        /// Path to wall types file.
        /// </summary>
        public string WallFilePath { get; }
        /// <summary>
        /// Path to game map file.
        /// </summary>
        public string MapFilePath { get; }
        /// <summary>
        /// Path to enemy types file.
        /// </summary>
        public string EnemyFilePath { get; }
        /// <summary>
        /// Path to all weapon types file.
        /// </summary>
        public string WeaponFilePath { get; }
        /// <summary>
        /// Path to player data file.
        /// </summary>
        public string PlayerFilePath { get; }
        /// <summary>
        /// Path to sky texture.
        /// </summary>
        public string SkyFilePath { get; }
        /// <summary>
        /// Path to floor texture.
        /// </summary>
        public string FloorFilePath { get; }

        /// <summary>
        /// Determines how often do enemy spawn times change.
        /// </summary>
        public int EnemySpawnSecs { get; }

        /// <summary>
        /// Default raycasting condition.
        /// </summary>
        public ICastCondition Condition { get; }
        /// <summary>
        /// Background texture width.
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// Background texture height.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Initializes a new game settings instance.
        /// </summary>
        /// <param name="wallFilePath">Path to wall input file.</param>
        /// <param name="mapFilePath">Path to map input file.</param>
        /// <param name="enemyFilePath">Path to enemies input file.</param>
        /// <param name="weaponFilePath">Path to weapon input file.</param>
        /// <param name="playerFilePath">Path to player data input file.</param>
        /// <param name="skyFilePath">Path to sky texture.</param>
        /// <param name="floorFilePath">Path to floor texture.</param>
        /// <param name="condition">Stop condition for raycasting.</param>
        /// <param name="enemySpawnSecs">Enemy spawn change interval in seconds</param>
        /// <param name="width">Background texture width.</param>
        /// <param name="height">Background texture height.</param>
        public EngineSettings(string wallFilePath, string mapFilePath, string enemyFilePath, string weaponFilePath, string playerFilePath, string skyFilePath, string floorFilePath,
            ICastCondition condition, int enemySpawnSecs, int width, int height)
        {
            WallFilePath = wallFilePath;
            MapFilePath = mapFilePath;
            EnemyFilePath = enemyFilePath;
            WeaponFilePath = weaponFilePath;
            PlayerFilePath = playerFilePath;
            SkyFilePath = skyFilePath;
            FloorFilePath = floorFilePath;
            Condition = condition;
            EnemySpawnSecs = enemySpawnSecs;
            Width = width;
            Height = height;
        }
    }
}
