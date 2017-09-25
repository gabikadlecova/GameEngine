using System;
using System.Collections.Generic;
using Casting.Environment.Interfaces;
using Casting.RayCasting;
using Microsoft.Xna.Framework;

namespace Casting.Environment
{
    /// <summary>
    /// Holds basic information about a particular wall type
    /// </summary>
    public class Wall : IWall
    {
        /// <summary>
        /// Initializes a new wall type instance
        /// </summary>
        /// <param name="textureX">Texture of the x side</param>
        /// <param name="textureY">Texture of the y side</param>
        /// <param name="altX">X side alternative color</param>
        /// <param name="altY">Y side alternative color</param>
        /// <param name="height">Wall height</param>
        public Wall(string textureX, string textureY, Color altX, Color altY, int height)
        {
            Textures = new List<ITextureWrapper>
            {
                new TextureWrapper(textureX, altX),
                new TextureWrapper(textureY, altY)
            };
            Height = height;
        }

        /// <summary>
        /// Height of the wall
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Inherited from ICrossable, unused
        /// </summary>
        public int Width { get { return 0; } }

        /// <summary>
        /// Gets a texture on a specified wall side
        /// </summary>
        /// <param name="side">Side of the wall</param>
        /// <returns>Texture on a wall side</returns>
        public ITextureWrapper GetTexture(Side side)
        {
            switch (side)
            {
                case Side.SideX:
                    return TextureX;
                case Side.SideY:
                    return TextureY;
                default:

                    throw new ArgumentException("This side is not defined for this object.");
            }
        }

        /// <summary>
        /// List of wall textures
        /// </summary>
        public List<ITextureWrapper> Textures { get; }
        /// <summary>
        /// Texture on wall X side
        /// </summary>
        public ITextureWrapper TextureX { get { return Textures[0]; } }
        /// <summary>
        /// Texture on wall Y side
        /// </summary>
        public ITextureWrapper TextureY { get { return Textures[1]; } }
    }
}
