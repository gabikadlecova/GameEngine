using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.Player.Interfaces;
using Casting.RayCasting;
using Microsoft.Xna.Framework;

namespace Casting.Player
{
    public abstract class Person : IPerson
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; protected set; }
        public int HitPoints { get; set; }
        public HumanCastCondition MovementCondition { get; set; }

        protected Person(float positionX, float positionY, float directionX, float directionY, 
            int hitPoints, HumanCastCondition condition)
        {
            Position = new Vector2(positionX, positionY);
            Direction = new Vector2(directionX, directionY);
            HitPoints = hitPoints;
            MovementCondition = condition;
        }

        protected Person(Vector2 positon, Vector2 direction, int hitpoints, HumanCastCondition condition)
        {
            Position = positon;
            Direction = direction;
            HitPoints = hitpoints;
            MovementCondition = condition;
        }

        public bool IsKilled { get { return HitPoints <= 0; } }

        public virtual void Rotate(float angle)
        {
            Matrix rotation = Matrix.CreateRotationZ(angle);
            Direction = Vector2.Transform(Direction, rotation);
        }
    }
}
