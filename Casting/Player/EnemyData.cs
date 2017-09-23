using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.RayCasting.Interfaces;

namespace Casting.Player
{
    public class EnemyData
    {
        public EnemyData(int typeId, SpriteData spriteData, int hitPoints, float hitBox, float movementSpeed, float spawnTime)
        {
            TypeId = typeId;
            SpriteData = spriteData;
            HitPoints = hitPoints;
            HitBox = hitBox;
            MovementSpeed = movementSpeed;
            SpawnTime = spawnTime;
        }

        public IRayCaster Caster;
        public int TypeId { get; }
        public SpriteData SpriteData { get; }
        public int HitPoints { get; }
        public float HitBox { get; }
        public float MovementSpeed { get; }

        public float SpawnTime;
        public TimeSpan LastSpawn;

    }
}
