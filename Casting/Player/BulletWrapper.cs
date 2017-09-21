using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.Environment;
using Casting.Environment.Interfaces;
using Casting.RayCasting;
using Microsoft.Xna.Framework;

namespace Casting.Player
{
    public class BulletWrapper
    {
        public BulletWrapper(string flyPicAdd, string hitPicAdd, int size, float movementSpeed)
        {
            FlyPic = new TextureWrapper(flyPicAdd, Color.Transparent);
            HitPic = new TextureWrapper(hitPicAdd, Color.Transparent);
            Size = size;
            MovementSpeed = movementSpeed;
        }

        public ITextureWrapper FlyPic { get; }
        public ITextureWrapper HitPic { get; }
        
        public int Size { get; }

        public float MovementSpeed { get; }
    }
}
