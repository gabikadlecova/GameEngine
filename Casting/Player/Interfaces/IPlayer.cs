using Casting.RayCasting.Interfaces;

namespace Casting.Player.Interfaces
{
    public interface IPlayer
    {
        IVector Position { get; set; }

        IVector Direction { get; set; }

        int HitPoints { get; set; }

        IWeapon Weapon { get; set; }

        string Name { get; set; }

    }
}
