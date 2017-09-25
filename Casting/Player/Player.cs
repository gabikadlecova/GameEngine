using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.Player.Interfaces;
using Casting.RayCasting;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;

namespace Casting.Player
{
    /// <summary>
    /// Represents the game player
    /// </summary>
    public class Player : MovingObject, IPerson
    {
        /// <summary>
        /// Initializes a new player instance
        /// </summary>
        /// <param name="positionX">Initial position x coordinate</param>
        /// <param name="positionY">Initial position y coordinate</param>
        /// <param name="directionX">Initial direction x coordinate</param>
        /// <param name="directionY">Initial direction y coordinate</param>
        /// <param name="hitPoints">Initial</param>
        /// <param name="condition">Movement cast condition</param>
        /// <param name="name">Player name</param>
        /// <param name="movementSpeed">Player movement speed</param>
        /// <param name="caster">Default movement raycaster</param>
        public Player(float positionX, float positionY, float directionX, float directionY, int hitPoints, HumanCastCondition condition, string name, float movementSpeed, IRayCaster caster) 
            : base(positionX, positionY, directionX, directionY, condition, movementSpeed, caster)
        {
            Name = name;

            float planeX = directionY;
            float planeY = -directionX;
            HitPoints = hitPoints;
            Vector2 screenPlane = new Vector2(planeX, planeY);
            screenPlane.Normalize();
            ScreenPlane = screenPlane;
        }

        /// <summary>
        /// Initializes a new player instance
        /// </summary>
        /// <param name="position">Initial position vector</param>
        /// <param name="direction">Initial direction vector</param>
        /// <param name="hitPoints">Initial</param>
        /// <param name="condition">Movement cast condition</param>
        /// <param name="name">Player name</param>
        /// <param name="movementSpeed">Player movement speed</param>
        /// <param name="caster">Default movement raycaster</param>
        public Player(Vector2 position, Vector2 direction, int hitPoints, HumanCastCondition condition, string name, float movementSpeed, IRayCaster caster) 
            : base(position, direction, condition, movementSpeed, caster)
        {
            Name = name;

            float planeX = direction.Y;
            float planeY = -direction.X;
            HitPoints = hitPoints;
            Vector2 screenPlane = new Vector2(planeX, planeY);
            screenPlane.Normalize();
            ScreenPlane = screenPlane;

        }

        /// <summary>
        /// The weapon currently held by player
        /// </summary>
        public IWeapon Weapon { get; set; }
        /// <summary>
        /// Player name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Screen plane vector, which is perpendicular to the direction vector
        /// </summary>
        public Vector2 ScreenPlane { get; private set; }

        /// <summary>
        /// Rotates both the player direction and screenplane
        ///  </summary>
        /// <param name="angle">Rotation angle</param>
        public override void Rotate(float angle)
        {
            Matrix rotation = Matrix.CreateRotationZ(angle);
            ScreenPlane = Vector2.Transform(ScreenPlane, rotation);
            base.Rotate(angle);
        }

        /// <summary>
        /// Shoots a bullet from the current weapon
        /// </summary>
        public void Shoot()
        {
            Weapon.Shoot(Position, Direction);
        }

        /// <summary>
        /// Current player hit points
        /// </summary>
        public int HitPoints { get; set; }
        /// <summary>
        /// Returns true if the player hit points are less then or equal to zero
        /// </summary>
        public bool IsKilled { get { return HitPoints <= 0; } }
    }
}
