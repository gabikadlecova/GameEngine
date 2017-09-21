using System;
using Casting.RayCasting.Interfaces;

namespace Casting.RayCasting
{
    public class DistanceWrapper<TObj> where TObj : ICrossable
    {
        public double Distance { get; }

        public double? ObliqueDistance { get; }

        /// <summary>
        /// Defines the x coordinate of the texture, or precisely the exact location of the ray hit
        /// </summary>
        public double TextureXRatio { get; }

        /// <summary>
        /// Indicates on which side did the ray hit
        /// </summary>
        public Side Side { get; }
        public TObj Element { get; }

        public DistanceWrapper(double distance, double textureXRatio, Side side, TObj element, double? obliqueDistance = null)
        {
            Distance = distance;
            Element = element;
            TextureXRatio = textureXRatio;
            Side = side;
            ObliqueDistance = obliqueDistance;
        }
    }
}
