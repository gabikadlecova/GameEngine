using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.RayCasting;
using Microsoft.Xna.Framework;

namespace Casting.Player.Interfaces
{
    public interface IPerson
    {
        Vector2 Position { get; set; }

        Vector2 Direction { get; }

        int HitPoints { get; set; }

        HumanCastCondition MovementCondition { get; set; }

        void Rotate(float angle);
        bool IsKilled { get; }

    }
}
