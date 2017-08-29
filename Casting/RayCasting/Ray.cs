using System.Collections.Generic;
using Casting.Environment.Interfaces;
using Casting.RayCasting.Interfaces;

namespace Casting.RayCasting
{
    class Ray : IRay
    {
        public List<DistanceWrapper<IWall>> WallsCrossed { get; }


        public Ray()
        {
            WallsCrossed = new List<DistanceWrapper<IWall>>();
        }
    }
}
