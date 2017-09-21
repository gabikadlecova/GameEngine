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
    public abstract class MovingObject
    {
        protected const float MinWallDist = 0.3F;

        public Vector2 Position { get; set; }
        public Vector2 Direction { get; protected set; }
       
        public float MovementSpeed { get; set; }
        public HumanCastCondition MovementCondition { get; set; }

        public IRayCaster Caster;

        protected MovingObject(float positionX, float positionY, float directionX, float directionY,
             HumanCastCondition condition, float movementSpeed)
        {
            Position = new Vector2(positionX, positionY);
            Direction = new Vector2(directionX, directionY);
            MovementCondition = condition;
            MovementSpeed = movementSpeed;
        }

        protected MovingObject(Vector2 positon, Vector2 direction, HumanCastCondition condition, float movementSpeed)
        {
            Position = positon;
            Direction = direction;
            MovementCondition = condition;
            MovementSpeed = movementSpeed;
        }


        public virtual void Rotate(float angle)
        {
            Matrix rotation = Matrix.CreateRotationZ(angle);
            Direction = Vector2.Transform(Direction, rotation);
        }

        public virtual bool Move(Vector2 direction)
        {
            direction.Normalize();

            direction = MovementSpeed * direction;
            Vector2 nextPosition = Position + direction;

            float distance = direction.Length();

            if (distance > Double.Epsilon)
            {
                //todo minwalldist?
                MovementCondition.Reset(distance + MinWallDist);

                direction.Normalize();
                Ray resultRay = Caster.Cast(Position, direction, MovementCondition);

                if (resultRay.ObjectsCrossed.Count < 1 || resultRay.ObjectsCrossed[0].Distance > distance + MinWallDist)
                {
                    Position = nextPosition;
                    return true;
                }


                resultRay = Caster.Cast(Position, new Vector2(direction.X, 0), MovementCondition);
                if (resultRay.ObjectsCrossed.Count > 0 && resultRay.ObjectsCrossed[0].Distance <= distance + MinWallDist)
                {
                    nextPosition.X = Position.X;
                }

                resultRay = Caster.Cast(Position, new Vector2(0, direction.Y), MovementCondition);
                if (resultRay.ObjectsCrossed.Count > 0 && resultRay.ObjectsCrossed[0].Distance <= distance + MinWallDist)
                {
                    nextPosition.Y = Position.Y;
                }

                Position = nextPosition;

            }
            return false;
        }
    }
}
