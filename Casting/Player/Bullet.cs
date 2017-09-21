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
    public class Bullet : MovingObject, IBullet, ICrossable
    {
        public Bullet(Vector2 position, Vector2 direction, HumanCastCondition condition, BulletWrapper bulletData) : base(position, direction, condition, bulletData.MovementSpeed)
        {
            HasHit = false;
            Height = bulletData.Size;
            Width = bulletData.Size;
            FlyPic = bulletData.FlyPic;
            HitPic = bulletData.HitPic;

        }


        // public abstract Texture2D GetTexture();

        public virtual void Hit(TimeSpan explosionTime)
        {
            AnimationTime = explosionTime;
            HasHit = true;
        }

        public TimeSpan AnimationTime { get; private set; }
        public bool HasHit { get; private set; }


        public int Height { get; }
        public int Width { get; }
        public ITextureWrapper FlyPic { get; }
        public ITextureWrapper HitPic { get; }

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
