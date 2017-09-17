using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.Player.Interfaces;
using Casting.RayCasting;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;
using Ray = Casting.RayCasting.Ray;

namespace Casting.Player
{
    public abstract class Person : IPerson
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; protected set; }
        public int HitPoints { get; set; }
        public float MovementSpeed { get; set; }
        public HumanCastCondition MovementCondition { get; set; }

        public IRayCaster Caster;

        protected Person(float positionX, float positionY, float directionX, float directionY, 
            int hitPoints, HumanCastCondition condition, float movementSpeed)
        {
            Position = new Vector2(positionX, positionY);
            Direction = new Vector2(directionX, directionY);
            HitPoints = hitPoints;
            MovementCondition = condition;
            MovementSpeed = movementSpeed;
        }

        protected Person(Vector2 positon, Vector2 direction, int hitpoints, HumanCastCondition condition, float movementSpeed)
        {
            Position = positon;
            Direction = direction;
            HitPoints = hitpoints;
            MovementCondition = condition;
            MovementSpeed = movementSpeed;
        }

        public bool IsKilled { get { return HitPoints <= 0; } }

        public virtual void Rotate(float angle)
        {
            Matrix rotation = Matrix.CreateRotationZ(angle);
            Direction = Vector2.Transform(Direction, rotation);
        }

        public virtual void Move(Vector2 direction)
        {
            direction.Normalize();

            direction = MovementSpeed * direction;
            Vector2 nextPosition = Position + direction;

            float distance = direction.Length();

            if (distance > Double.Epsilon)
            {
                MovementCondition.ResetDistance(distance);

                direction.Normalize();
                Ray resultRay = Caster.Cast(Position, direction, MovementCondition);

                if (resultRay.ObjectsCrossed.Count < 1)
                {
                    Position = nextPosition;
                }
            }
        }
    }
}
