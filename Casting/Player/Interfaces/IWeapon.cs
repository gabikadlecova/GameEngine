using System.Collections.Generic;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Casting.Player.Interfaces
{
    public interface IWeapon
    {
        int MaxAmmo { get; }

        List<Bullet> Bullets { get; }

        BulletWrapper BulletData { get; }

        IRayCaster Caster { get; set; }

        //todo probably image/bitmap
        Texture2D Texture { get; set; }

        Bullet Shoot(Vector2 from, Vector2 direction);
    }
}
