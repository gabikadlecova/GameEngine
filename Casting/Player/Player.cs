﻿using System;
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
    public class Player : IPlayer
    {
        //todo constructor
        
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public int HitPoints { get; set; }
        public IWeapon Weapon { get; set; }
        public string Name { get; set; }
        public HumanCastCondition MovementCondition { get; set; }
    }
}
