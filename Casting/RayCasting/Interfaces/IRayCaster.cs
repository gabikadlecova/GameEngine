using System.ComponentModel;
using Casting.Environment.Interfaces;
using Microsoft.Xna.Framework;

namespace Casting.RayCasting.Interfaces
{
    public interface IRayCaster
    {
        IMap Map { get; set; }
        IContainer<IWall> Walls { get; set; }
        IRay Cast(Vector2 startPosition, Vector2 direction, ICastCondition condition);
    }
}
