using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.Environment.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace Casting.RayCasting.Interfaces
{
    /// <summary>
    /// Represents a game object that can be directly or indirectly crossed by a ray.
    /// </summary>
    public interface ICrossable
    {
        /// <summary>
        /// Height of the object
        /// </summary>
        int Height { get; }
        /// <summary>
        /// Width of the object
        /// </summary>
        int Width { get; }
        /// <summary>
        /// Gets a texture on the side of the object which has been hit.
        /// </summary>
        /// <param name="side">Side on which has the object been hit</param>
        /// <returns>Texture that corresponds to the side parameter</returns>
        ITextureWrapper GetTexture(Side side);

    }
}
