using Casting.RayCasting;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;
using Ray = Casting.RayCasting.Ray;

namespace Casting.Player
{
    /// <summary>
    ///     This class represents an object that can move in the game environment
    /// </summary>
    public abstract class MovingObject
    {
        /// <summary>
        ///     Minimum distance from an obstacle while moving forward
        /// </summary>
        protected const float MinWallDist = 0.3F;

        /// <summary>
        /// Initializes a new instance of a moving object
        /// </summary>
        /// <param name="positionX">Initial position x coordinate</param>
        /// <param name="positionY">Initial position y coordinate</param>
        /// <param name="directionX">Initial direction x coordinate</param>
        /// <param name="directionY">Initial direction y coordinate</param>
        /// <param name="condition">Movement cast condition</param>
        /// <param name="movementSpeed">Movement speed multiplier</param>
        /// <param name="caster">Default movement raycaster</param>
        protected MovingObject(float positionX, float positionY, float directionX, float directionY,
            HumanCastCondition condition, float movementSpeed, IRayCaster caster)
        {
            Position = new Vector2(positionX, positionY);
            Direction = new Vector2(directionX, directionY);
            MovementCondition = condition;
            MovementSpeed = movementSpeed;
            Caster = caster;
        }

        /// <summary>
        /// Initializes a new instance of a moving object
        /// </summary>
        /// <param name="position">Initial position vector</param>
        /// <param name="direction">Initial direction vector</param>
        /// <param name="condition">Movement cast condition</param>
        /// <param name="movementSpeed">Movement speed multiplier</param>
        /// <param name="caster">Default movement raycaster</param>
        protected MovingObject(Vector2 position, Vector2 direction, HumanCastCondition condition, float movementSpeed,
            IRayCaster caster)
        {
            Position = position;
            Direction = direction;
            MovementCondition = condition;
            MovementSpeed = movementSpeed;
            Caster = caster;
        }

        /// <summary>
        ///     Current position
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        ///     Current movement direction
        /// </summary>
        public Vector2 Direction { get; protected set; }

        /// <summary>
        ///     Movement speed multiplier
        /// </summary>
        public float MovementSpeed { get; set; }

        /// <summary>
        ///     Default movement cast condition for raycasting
        /// </summary>
        public HumanCastCondition MovementCondition { get; set; }

        private IRayCaster Caster { get; }

        /// <summary>
        /// Rotates the direction vector of the object
        /// </summary>
        /// <param name="angle">Rotation angle</param>
        public virtual void Rotate(float angle)
        {
            Matrix rotation = Matrix.CreateRotationZ(angle);
            Direction = Vector2.Transform(Direction, rotation);
        }

        /// <summary>
        /// Moves the object along the direction vector multiplied by movement speed multiplier
        /// </summary>
        /// <param name="direction">Direction of the movement</param>
        /// <returns>Returns true if the object had been able to move</returns>
        public virtual bool Move(Vector2 direction)
        {
            direction.Normalize();

            direction = MovementSpeed * direction;
            Vector2 nextPosition = Position + direction;

            float distance = direction.Length();

            if (distance > double.Epsilon)
            {
                MovementCondition.Reset(distance + MinWallDist);

                direction.Normalize();
                Ray resultRay = Caster.Cast(Position, direction, MovementCondition);

                //can the object move forward?
                if (resultRay.ObjectsCrossed.Count < 1 || resultRay.ObjectsCrossed[0].Distance > distance + MinWallDist)
                {
                    Position = nextPosition;
                    return true;
                }

                //can the object move at least in the direction of x or y axis?

                resultRay = Caster.Cast(Position, new Vector2(direction.X, 0), MovementCondition);
                if (resultRay.ObjectsCrossed.Count > 0 && resultRay.ObjectsCrossed[0].Distance <=
                    distance + MinWallDist)
                    nextPosition.X = Position.X;

                resultRay = Caster.Cast(Position, new Vector2(0, direction.Y), MovementCondition);
                if (resultRay.ObjectsCrossed.Count > 0 && resultRay.ObjectsCrossed[0].Distance <=
                    distance + MinWallDist)
                    nextPosition.Y = Position.Y;

                Position = nextPosition;
            }
            return false;
        }
    }
}