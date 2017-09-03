using System.Collections.Generic;
using Casting.Environment.Interfaces;
using Casting.RayCasting.Interfaces;

namespace Casting.RayCasting
{
    class Ray : IRay
    {
        public List<DistanceWrapper<ICrossable>> ObjectsCrossed { get; }


        public Ray()
        {
            ObjectsCrossed = new List<DistanceWrapper<ICrossable>>();
        }
    }
}
