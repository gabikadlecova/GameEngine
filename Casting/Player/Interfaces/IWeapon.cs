using System.Collections.Generic;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Casting.Player.Interfaces
{
    /// <summary>
    /// Represents a weapon in the game
    /// </summary>
    public interface IWeapon
    {
        /// <summary>
        /// Maximum bullets that can be shot
        /// </summary>
        int MaxAmmo { get; }

        /// <summary>
        /// List of bullets that have been shot
        /// </summary>
        List<Bullet> Bullets { get; }

        /// <summary>
        /// Minimum distance between bullets
        /// </summary>
        float MinBulletDist { get; }

        /// <summary>
        /// Default bullet sprite data
        /// </summary>
        SpriteData BulletData { get; }

        /// <summary>
        /// Default raycaster for bullet movement
        /// </summary>
        IRayCaster Caster { get; }

        /// <summary>
        /// Weapon pixture
        /// </summary>
        Texture2D Texture { get; set; }

        /// <summary>
        /// Shoots a new bullet if it is possible
        /// </summary>
        /// <param name="from"> Position from where it should be shot</param>
        /// <param name="direction">Shooting direction</param>
        /// <returns>The bullet that has just been shot</returns>
        Bullet Shoot(Vector2 from, Vector2 direction);
    }
}
