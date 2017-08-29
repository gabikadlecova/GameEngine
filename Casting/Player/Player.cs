using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.Player.Interfaces;
using Casting.RayCasting.Interfaces;

namespace Casting.Player
{
    public class Player : IPlayer
    {
        

        public IVector Position { get; set; }
        public IVector Direction { get; set; }
        public int HitPoints { get; set; }
        public IWeapon Weapon { get; set; }
        public string Name { get; set; }
    }
}
