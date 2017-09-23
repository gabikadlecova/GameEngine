using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.Player.Interfaces;
using Casting.RayCasting;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Casting.Player
{
    public class BasicWeapon : IWeapon
    {
        public BasicWeapon(int maxAmmo, string picAddress, SpriteData bulletGraphics, float movementSpeed)
        {
            MaxAmmo = maxAmmo;
            Bullets = new List<Bullet>();
            PicAddress = picAddress;
            BulletData = bulletGraphics;
            MovementSpeed = movementSpeed;

        }

        public float MovementSpeed { get; }
        public SpriteData BulletData { get; }
        public IRayCaster Caster { get; set; }
        public Texture2D Texture { get; set; }
        

        public int MaxAmmo { get; }

        public List<Bullet> Bullets { get; }
        public string PicAddress { get; set; }
        public Bullet Shoot(Vector2 from, Vector2 direction)
        {
            if (Bullets.Count < MaxAmmo)
            { 
                Bullet bullet = new Bullet(from, direction, HumanCastCondition.Default(), MovementSpeed, BulletData) { Caster = Caster };
                Bullets.Add(bullet);
                return bullet;
            }
            return null;
        }
    }
}
