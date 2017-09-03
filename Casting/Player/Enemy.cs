using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.Environment.Interfaces;
using Casting.Player.Interfaces;
using Casting.RayCasting;
using Microsoft.Xna.Framework;

namespace Casting.Player
{
    class Enemy : IEnemy
    {
        //todo constructor

        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public int HitPoints { get; set; }
        public HumanCastCondition MovementCondition { get; set; }
        public int Height { get; }
        public ITextureWrapper GetTexture(Side side)
        {
            return Texture;
        }

        public int EnemyId { get; }
        public int TypeId { get; }
        public ITextureWrapper Texture { get; }

        public bool IsKilled { get { return HitPoints <= 0; } }
    }
}
