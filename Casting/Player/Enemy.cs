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
using Ray = Casting.RayCasting.Ray;

namespace Casting.Player
{
    /// <summary>
    /// This class represents an enemy which can move and be drawn on the screen plane
    /// </summary>
    public class Enemy : MovingObject, ICrossable, IPerson
    {
        /// <summary>
        /// Initializes a new enemy instance
        /// </summary>
        /// <param name="positionX">Enemy position x coordinate</param>
        /// <param name="positionY">Enemy position y coordinate</param>
        /// <param name="directionX">Enemy direction x coordinate</param>
        /// <param name="directionY">Enemy direction y coordinate</param>
        /// <param name="condition">Cast condition for movement raycasting</param>
        /// <param name="enemyData">Provides sprite, spawn and other data </param>
        /// <param name="caster">Default movement raycaster</param>
        public Enemy(float positionX, float positionY, float directionX, float directionY, 
            HumanCastCondition condition, EnemyData enemyData, IRayCaster caster) : base(positionX, positionY, directionX, directionY, condition, enemyData.MovementSpeed, caster)
        {
            SpriteData spriteData = enemyData.SpriteData;
            Height = spriteData.Height;
            Width = spriteData.Width;
            Texture = spriteData.LivingPic;
            KilledTexture = spriteData.DeadPic;

            TypeId = enemyData.TypeId;
            HitPoints = enemyData.HitPoints;
            HitBox = enemyData.HitBox;
        }
        /// <summary>
        /// Initializes a new enemy instance
        /// </summary>
        /// <param name="position">Enemy starting position</param>
        /// <param name="direction">Enemy direction</param>
        /// <param name="condition">Cast condition for movement raycasting</param>
        /// <param name="enemyData">Provides sprite, spawn and other data </param>
        /// <param name="caster">Default movement raycaster</param>
        public Enemy(Vector2 position, Vector2 direction, HumanCastCondition condition, EnemyData enemyData, IRayCaster caster) : base(position, direction, condition, enemyData.MovementSpeed, caster)
        {
            SpriteData spriteData = enemyData.SpriteData;
            Height = spriteData.Height;
            Width = spriteData.Width;
            Texture = spriteData.LivingPic;
            KilledTexture = spriteData.DeadPic;

            TypeId = enemyData.TypeId;
            HitPoints = enemyData.HitPoints;
            HitBox = enemyData.HitBox;
        }
        /// <summary>
        /// Hit box radius
        /// </summary>
        public float HitBox { get; }

        /// <summary>
        /// Enemy height
        /// </summary>
        public int Height { get; }
        /// <summary>
        /// Enemy width
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the current enemy texture
        /// </summary>
        /// <param name="side">Unused side parameter, this enemy can be seen only from one side</param>
        /// <returns>The enemy's living or dead texture</returns>
        public ITextureWrapper GetTexture(Side side)
        {
            return IsKilled ? KilledTexture : Texture;
        }
        /// <summary>
        /// Enemy type id
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        /// Texture that is used for living enemies
        /// </summary>
        public ITextureWrapper Texture { get; }

        /// <summary>
        /// Texture that is used for enemies that had been killed
        /// </summary>
        public ITextureWrapper KilledTexture { get; }

        /// <summary>
        /// Enemy hit points
        /// </summary>
        public int HitPoints { get; set; }

        /// <summary>
        /// Returns true if the enemy hitpoints are less then or equal to zero
        /// </summary>
        public bool IsKilled { get { return HitPoints <= 0; } }

        /// <summary>
        /// Determines the death time offset from the game start in seconds
        /// </summary>
        public int DeathSecs;
    }
}
