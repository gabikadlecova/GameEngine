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

        public ICastCondition Condition;
        
    }
}
