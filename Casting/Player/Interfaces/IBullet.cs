using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Casting.Player.Interfaces
{
    public interface IBullet
    {
        //Texture2D GetTexture();

            
        void Hit(TimeSpan explosionTime);

        TimeSpan AnimationTime { get; }

        float MinBulletDist { get; }

        bool HasHit { get; }
    }
}