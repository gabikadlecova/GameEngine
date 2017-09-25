using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.RayCasting;
using Microsoft.Xna.Framework;

namespace Casting.Player.Interfaces
{
    /// <summary>
    /// A person in the game
    /// </summary>
    public interface IPerson
    {
        /// <summary>
        /// Current hit points
        /// </summary>
       int HitPoints { get; set; }

        /// <summary>
        /// Is true if the person has been killed
        /// </summary>
       bool IsKilled { get; }

    }
}
