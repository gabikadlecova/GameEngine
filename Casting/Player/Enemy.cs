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

        private readonly Random _random = new Random();
        private CastCondition _wallCondition = CastCondition.CastDistance(MinWallDist, 1);

        public Enemy(float positionX, float positionY, float directionX, float directionY, int hitPoints,
            HumanCastCondition condition, int height, int width, int typeId,
            string texturePath, string killedTexture, float movementSpeed, float hitBox) : base(positionX, positionY, directionX, directionY, condition, movementSpeed)
        {
            Height = height;
            Width = width;
            TypeId = typeId;

            HitPoints = hitPoints;
            HitBox = hitBox;
            Texture = new TextureWrapper(texturePath, Color.Transparent);
            KilledTexture = new TextureWrapper(killedTexture, Color.Transparent);
        }

        public Enemy(Vector2 positon, Vector2 direction, int hitpoints, HumanCastCondition condition, int height, int width, int typeId, 
            string texturePath, string killedTexture, float movementSpeed, float hitBox) : base(positon, direction, condition, movementSpeed)
        {
            Height = height;
            Width = width;
            TypeId = typeId;

            HitPoints = hitpoints;
            HitBox = hitBox;
            Texture = new TextureWrapper(texturePath, Color.Transparent);
            KilledTexture = new TextureWrapper(killedTexture, Color.Transparent);
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
    }
}
