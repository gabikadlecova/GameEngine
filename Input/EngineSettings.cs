using Casting.Environment;
using Casting.Environment.Interfaces;
using Casting.Player.Interfaces;
using Casting.RayCasting.Interfaces;

namespace Input
{
    public class EngineSettings
    {
        public IPlayer Player;
        public string WallFilePath;
        public ICastCondition Condition;
        public string MapFilePath;
        public IVector ScreenPlane;
        
    }
}
