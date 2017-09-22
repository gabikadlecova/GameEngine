using Casting.Player.Interfaces;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;

namespace Input
{
    public class EngineSettings
    {
        public string WallFilePath;
        public string MapFilePath;
        public string EnemyFilePath;
        public string WeaponFilePath;

        public string SkyFilePath;
        public string FloorFilePath;

        public ICastCondition Condition;
        
    }
}
