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
    public class Player : Person
    {
        //todo constructor

        public Player(float positionX, float positionY, float directionX, float directionY, int hitPoints, HumanCastCondition condition, string name) : base(positionX, positionY, directionX, directionY, hitPoints, condition)
        {
            Name = name;

            float planeX = directionY;
            float planeY = -directionX;
            Vector2 screenPlane = new Vector2(planeX, planeY);
            screenPlane.Normalize();
            ScreenPlane = screenPlane;
        }

        public Player(Vector2 positon, Vector2 direction, int hitpoints, HumanCastCondition condition, string name) : base(positon, direction, hitpoints, condition)
        {
           Name = name;


            float planeX = direction.Y;
            float planeY = -direction.X;
            Vector2 screenPlane = new Vector2(planeX, planeY);
            screenPlane.Normalize();
            ScreenPlane = screenPlane;

        }

        public IWeapon Weapon { get; set; }
        public string Name { get; set; }
        //todo
        public Vector2 ScreenPlane { get; private set; }

        public override void Rotate(float angle)
        {
            Matrix rotation = Matrix.CreateRotationZ(angle);
            ScreenPlane = Vector2.Transform(ScreenPlane, rotation);
            base.Rotate(angle);
        }
    }
}
