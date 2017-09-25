using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Casting.Player.Interfaces
{
    /// <summary>
    /// Provides additional data of a flying bullet
    /// </summary>
    public interface IBullet
    {
        /// <summary>
        /// Starts the bullet explosion.
        /// </summary>
        /// <param name="explosionTime">Time of the hit</param>
        void Hit(TimeSpan explosionTime);

        /// <summary>
        /// The point in time when the bullet started exploding.
        /// </summary>
        TimeSpan AnimationTime { get; }

        /// <summary>
        /// Is true if the bullet has already hit
        /// </summary>
        bool HasHit { get; }
    }
}