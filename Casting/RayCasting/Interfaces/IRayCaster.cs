using Casting.Environment.Interfaces;

namespace Casting.RayCasting.Interfaces
{
    public interface IRayCaster
    {
        IMap Map { get; set; }
        IWallContainer Walls { get; set; }
        IRay Cast(IVector startPosition, IVector direction, ICastCondition condition);
    }
}
