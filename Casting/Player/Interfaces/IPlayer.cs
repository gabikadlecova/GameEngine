using Casting.RayCasting;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;

namespace Casting.Player.Interfaces
{
    public interface IPlayer : IPerson
    {

        IWeapon Weapon { get; set; }

        string Name { get; set; }

    }
}
