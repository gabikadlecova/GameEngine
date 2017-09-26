using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Casting.Player.Interfaces;
using Casting.RayCasting;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Casting.Player
{
    /// <summary>
    /// Basic weapon class which enables bullet shooting
    /// </summary>
    public class BasicWeapon : IWeapon
    {
        /// <summary>
        /// Initializes a new weapon instance
        /// </summary>
        /// <param name="maxAmmo">Maximum bullets to be shot</param>
        /// <param name="picAddress">Weapon texture path</param>
        /// <param name="bulletGraphics">Bullet sprite data</param>
        /// <param name="movementSpeed">Bullet movement speed</param>
        /// <param name="minDist">Minimum bullet distance</param>
        /// <param name="caster">Bullet movement raycaster</param>
        public BasicWeapon(int maxAmmo, string picAddress, SpriteData bulletGraphics, float movementSpeed, float minDist, IRayCaster caster)
        {
            MaxAmmo = maxAmmo;
            Bullets = new List<Bullet>();
            PicAddress = picAddress;
            BulletData = bulletGraphics;
            MovementSpeed = movementSpeed;
            Caster = caster;
            MinBulletDist = minDist;

        }

        /// <summary>
        /// Determines how fast do the bullets move
        /// </summary>
        public float MovementSpeed { get; }
        /// <summary>
        /// Holds bullet sprite data
        /// </summary>
        public SpriteData BulletData { get; }
        /// <summary>
        /// Default bullet movement raycaster
        /// </summary>
        public IRayCaster Caster { get; }
        /// <summary>
        /// Weapon texture
        /// </summary>
        public Texture2D Texture { get; set; }
        /// <summary>
        /// Minimum bullet distance, a bullet can be shot only if the last bullet is farther than this distance.
        /// </summary>
        public float MinBulletDist { get; }

        /// <summary>
        /// Maximum bullets that can be shot at a time
        /// </summary>
        public int MaxAmmo { get; }

        /// <summary>
        /// Current bullets that have been shot
        /// </summary>
        public List<Bullet> Bullets { get; }
        /// <summary>
        /// Weapon texture path
        /// </summary>
        public string PicAddress { get; set; }

        /// <summary>
        /// Shoot a new bullet in a specified direction.
        /// </summary>
        /// <param name="from">Starting position</param>
        /// <param name="direction">Shooting direction</param>
        /// <returns>Returns the shot bullet</returns>
        public Bullet Shoot(Vector2 from, Vector2 direction)
        {
            if (Bullets.Count < MaxAmmo)
            { 
                Bullet bullet = new Bullet(from, direction, HumanCastCondition.Default(), MovementSpeed, BulletData, Caster);
                Bullets.Add(bullet);
                return bullet;
            }
            return null;
        }
    }
}
