using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.Environment;
using Casting.Environment.Interfaces;
using Casting.Player.Interfaces;
using Casting.RayCasting;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Casting.Player
{
    /// <summary>
    /// Represents a bullet that can be shot and hit other objects
    /// </summary>
    public class Bullet : MovingObject, IBullet, ICrossable
    {
        /// <summary>
        /// Initializes a new bullet instance
        /// </summary>
        /// <param name="position">Initial bullet position</param>
        /// <param name="direction">Bullet direction</param>
        /// <param name="condition">Movement cast condition</param>
        /// <param name="movementSpeed">Bullet movement speed</param>
        /// <param name="bulletData">Bullet sprite data</param>
        /// <param name="caster">Default movement raycaster</param>
        public Bullet(Vector2 position, Vector2 direction, HumanCastCondition condition, float movementSpeed, SpriteData bulletData, IRayCaster caster) : base(position, direction, condition, movementSpeed, caster)
        {
            HasHit = false;
            Height = bulletData.Height;
            Width = bulletData.Width;
            FlyPic = bulletData.LivingPic;
            HitPic = bulletData.DeadPic;

        }
        
        /// <summary>
        /// This method should be called when a bullet hits an obstacle and explodes
        /// </summary>
        /// <param name="explosionTime">Time offset from the game start when the explosion happens</param>
        public virtual void Hit(TimeSpan explosionTime)
        {
            AnimationTime = explosionTime;
            HasHit = true;
        }

        /// <summary>
        /// Time of the bullet explosion
        /// </summary>
        public TimeSpan AnimationTime { get; private set; }
        /// <summary>
        /// Is true if the bullet has already hit an obstacle
        /// </summary>
        public bool HasHit { get; private set; }

        /// <summary>
        /// Height of the bullet sprite
        /// </summary>
        public int Height { get; }
        /// <summary>
        /// Width of the bullet sprite
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// Texture for a flying bullet
        /// </summary>
        public ITextureWrapper FlyPic { get; }
        /// <summary>
        /// Texture for a bullet that is exploding
        /// </summary>
        public ITextureWrapper HitPic { get; }

        /// <summary>
        /// Returns the current texture
        /// </summary>
        /// <param name="side">Unused parameter, the side of the bullet we see (only one)</param>
        /// <returns>Current texture</returns>
        public ITextureWrapper GetTexture(Side side)
        {
            if (HasHit)
            {
                return HitPic;
            }

            return FlyPic;
        }
    }
}
