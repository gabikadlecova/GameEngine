using System.Collections.Generic;
using Casting.Environment.Interfaces;

namespace Casting.RayCasting.Interfaces
{
    public interface IRay
    {
        List<DistanceWrapper<IWall>> WallsCrossed { get; }
    }
}
