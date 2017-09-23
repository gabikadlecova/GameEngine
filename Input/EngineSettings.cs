using Casting.Player.Interfaces;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;

namespace Input
{
    public class EngineSettings
    {
        public string WallFilePath { get; }
        public string MapFilePath { get; }
        public string EnemyFilePath { get; }
        public string WeaponFilePath { get; }
        public string PlayerFilePath { get; }

        public string SkyFilePath { get; }
        public string FloorFilePath { get; }

        public ICastCondition Condition { get; }

        public EngineSettings(string wallFilePath, string mapFilePath, string enemyFilePath, string weaponFilePath, string playerFilePath, string skyFilePath, string floorFilePath, ICastCondition condition)
        {
            WallFilePath = wallFilePath;
            MapFilePath = mapFilePath;
            EnemyFilePath = enemyFilePath;
            WeaponFilePath = weaponFilePath;
            PlayerFilePath = playerFilePath;
            SkyFilePath = skyFilePath;
            FloorFilePath = floorFilePath;
            Condition = condition;
        }
    }
}
