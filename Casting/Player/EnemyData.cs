using System;
using Casting.RayCasting.Interfaces;

namespace Casting.Player
{
    /// <summary>
    ///     Contains data about a specific enemy type
    /// </summary>
    public class EnemyData
    {
        /// <summary>
        /// Last spawn of this particular enemy type
        /// </summary>
        public TimeSpan LastSpawn;
        /// <summary>
        /// Time offset in seconds between spawns
        /// </summary>
        public float SpawnTime;

        /// <summary>
        ///     Initializes a new enemy data type
        /// </summary>
        /// <param name="typeId">Identifier of the enemy type</param>
        /// <param name="spriteData">Enemy sprite data</param>
        /// <param name="hitPoints">Maximum hit points</param>
        /// <param name="hitBox">Hit box radius</param>
        /// <param name="movementSpeed">Enemy movement speed</param>
        /// <param name="spawnTime">Starting spawn time offset in seconds</param>
        /// <param name="caster">Default movement raycaster</param>
        public EnemyData(int typeId, SpriteData spriteData, int hitPoints, float hitBox, float movementSpeed,
            float spawnTime, IRayCaster caster)
        {
            TypeId = typeId;
            SpriteData = spriteData;
            HitPoints = hitPoints;
            HitBox = hitBox;
            MovementSpeed = movementSpeed;
            SpawnTime = spawnTime;
            Caster = caster;
        }

        /// <summary>
        ///     Default movement raycaster
        /// </summary>
        public IRayCaster Caster { get; }

        /// <summary>
        ///     Identifier of the enemy type
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        ///     Sprite data of this enemy type
        /// </summary>
        public SpriteData SpriteData { get; }

        /// <summary>
        ///     Current hit points
        /// </summary>
        public int HitPoints { get; }

        /// <summary>
        /// Hit box radius
        /// </summary>
        public float HitBox { get; }

        /// <summary>
        /// Enemy movement speed
        /// </summary>
        public float MovementSpeed { get; }
    }
}