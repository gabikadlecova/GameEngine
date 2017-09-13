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

namespace Casting.Player
{
    public class Enemy : Person, ICrossable
    {
        //todo constructor

        public Enemy(float positionX, float positionY, float directionX, float directionY, int hitPoints, HumanCastCondition condition, int height, int width, int typeId, string texturePath) : base(positionX, positionY, directionX, directionY, hitPoints, condition)
        {
            Height = height;
            Width = width;
            TypeId = typeId;

            Texture = new TextureWrapper(texturePath, Color.Transparent);
        }

        public Enemy(Vector2 positon, Vector2 direction, int hitpoints, HumanCastCondition condition, int height, int width, int typeId, string texturePath) : base(positon, direction, hitpoints, condition)
        {
            Height = height;
            Width = width;
            TypeId = typeId;

            Texture = new TextureWrapper(texturePath, Color.Transparent);
        }
        
        
        public int Height { get; }
        public int Width { get; }

        public ITextureWrapper GetTexture(Side side)
        {
            return Texture;
        }

        public int TypeId { get; }
        public ITextureWrapper Texture { get; }

    }
}
