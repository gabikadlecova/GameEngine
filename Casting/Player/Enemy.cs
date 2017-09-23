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
    public class Enemy : MovingObject, ICrossable, IPerson
    {

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

        public Enemy(Vector2 positon, Vector2 direction, HumanCastCondition condition, EnemyData enemyData, IRayCaster caster) : base(positon, direction, condition, enemyData.MovementSpeed, caster)
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

        public float HitBox { get; }

        public int Height { get; }
        public int Width { get; }

        public ITextureWrapper GetTexture(Side side)
        {
            return IsKilled ? KilledTexture : Texture;
        }

        public int TypeId { get; }
        public ITextureWrapper Texture { get; }

        public ITextureWrapper KilledTexture { get; }

        public int HitPoints { get; set; }
        public bool IsKilled { get { return HitPoints <= 0; } }
        public int DeathSecs;
    }
}
