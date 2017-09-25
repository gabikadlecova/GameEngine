using System.ComponentModel;
using Casting.Environment.Interfaces;
using Microsoft.Xna.Framework;

namespace Casting.RayCasting.Interfaces
{
    /// <summary>
    /// This class contains data of the map and walls and provides raycasting on this environment.
    /// </summary>
    public interface IRayCaster
    {
        /// <summary>
        /// Game map where the raycasting occurs.
        /// </summary>
        IMap Map { get; set; }
        /// <summary>
        /// Possible wall types.
        /// </summary>
        IContainer<IWall> Walls { get; set; }
        /// <summary>
        /// The raycaster casts a ray from a starting position into a specified direction and returns data of crossed objects after a certain Cast condition is met.
        /// </summary>
        /// <param name="startPosition">Position from which the ray is casted.</param>
        /// <param name="direction">Direction of the ray.</param>
        /// <param name="condition">Specifies when the raycasting should end.</param>
        /// <returns></returns>
        Ray Cast(Vector2 startPosition, Vector2 direction, ICastCondition condition);
    }
}
