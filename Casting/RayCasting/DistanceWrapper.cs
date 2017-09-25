using System;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;

namespace Casting.RayCasting
{
    /// <summary>
    /// Contains distance and position data about a crossed object.
    /// </summary>
    /// <typeparam name="TObj"></typeparam>
    public class DistanceWrapper<TObj> where TObj : ICrossable
    {
        /// <summary>
        /// Distance of the crossed object
        /// </summary>
        public double Distance { get; }

        /// <summary>
        /// Determines whether the object is partly transparent or not
        /// </summary>
        public bool IsNotTransparent { get; }

        /// <summary>
        /// Position of the object
        /// </summary>
        public Vector2 ElementPos { get; }

        /// <summary>
        /// Defines the x coordinate of the texture, or precisely the exact location of the ray hit
        /// </summary>
        public double TextureXRatio { get; }

        /// <summary>
        /// Indicates on which side did the ray hit
        /// </summary>
        public Side Side { get; }
        /// <summary>
        /// Crossed element
        /// </summary>
        public TObj Element { get; }

        /// <summary>
        /// Initializes a new instance of the wrapper
        /// </summary>
        /// <param name="distance">Distance of the crossed element</param>
        /// <param name="textureXRatio">Position of the ray hit</param>
        /// <param name="side">Hit side</param>
        /// <param name="element">Crossed element</param>
        /// <param name="elementPos">Position of the element</param>
        /// <param name="isNotTransparent">Should be set true for textures with possible transparency</param>
        public DistanceWrapper(double distance, double textureXRatio, Side side, TObj element, Vector2 elementPos, bool isNotTransparent)
        {
            Distance = distance;
            Element = element;
            TextureXRatio = textureXRatio;
            Side = side;
            ElementPos = elementPos;
            IsNotTransparent = isNotTransparent;
        }
    }
}
