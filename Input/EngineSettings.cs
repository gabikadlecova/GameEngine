using Casting.Player.Interfaces;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;

namespace Input
{
    public class EngineSettings
    {
        public IPlayer Player;
        public string WallFilePath;
        public ICastCondition Condition;
        public ICastCondition HumanCondition;
        public string MapFilePath;
        public Vector2 ScreenPlane;
        
    }
}
