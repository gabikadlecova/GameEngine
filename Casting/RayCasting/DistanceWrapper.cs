using System;
using Casting.RayCasting.Interfaces;

namespace Casting.RayCasting
{
    public class DistanceWrapper<TObj> : IComparable<DistanceWrapper<TObj>> where TObj : ICrossable
    {
        public double Distance { get; }

        /// <summary>
        /// Defines the x coordinate of the texture, or precisely the exact location of the ray hit
        /// </summary>
        public double TextureXRatio { get; }

        /// <summary>
        /// Indicates on which side did the ray hit
        /// </summary>
        public Side Side { get; }
        public TObj Element { get; }

        public DistanceWrapper(double distance, double textureXRatio, Side side, TObj element)
        {
            Distance = distance;
            Element = element;
            TextureXRatio = textureXRatio;
            Side = side;
        }

        public int CompareTo(DistanceWrapper<TObj> other)
        {
            if (Math.Abs(Distance - other.Distance) < 2E-12)
                return 0;
            if (Distance > other.Distance)
                return -1;
            return 1;
        }
    }
}
