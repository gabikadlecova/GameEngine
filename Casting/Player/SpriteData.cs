using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.Environment;
using Casting.Environment.Interfaces;
using Casting.RayCasting;
using Microsoft.Xna.Framework;

namespace Casting.Player
{
    /// <summary>
    /// Wrapper class for sprite related data
    /// </summary>
    public class SpriteData
    {
        /// <summary>
        /// Initializes a new sprite data wrapper class
        /// </summary>
        /// <param name="livePic">Texture for living objects</param>
        /// <param name="deadPic">Texture for dead objects</param>
        /// <param name="height">Height of the object's texture</param>
        /// <param name="width">Width of the object's texture</param>
        public SpriteData(string livePic, string deadPic, int height, int width)
        {
            LivingPic = new TextureWrapper(livePic, Color.Transparent);
            DeadPic = new TextureWrapper(deadPic, Color.Transparent);
            Height = height;
            Width = width;
        }

        /// <summary>
        /// Texture for living objects
        /// </summary>
        public ITextureWrapper LivingPic { get; }
        /// <summary>
        /// Alternative texture, can be used for dead objects
        /// </summary>
        public ITextureWrapper DeadPic { get; }
        
        /// <summary>
        /// Object texture height
        /// </summary>
        public int Height { get; }
        /// <summary>
        /// Object texture width
        /// </summary>
        public int Width { get; }

    }
}
