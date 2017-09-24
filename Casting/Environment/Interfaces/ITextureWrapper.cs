using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Casting.Environment.Interfaces
{
    /// <summary>
    /// Encapsulates raw texture pixel data
    /// </summary>
    public interface ITextureWrapper
    {
        /// <summary>
        /// Texture path
        /// </summary>
        string PicAddress { get; }

        /// <summary>
        /// This Color can be used if the texture isn't available.
        /// </summary>
        Color AltColor { get; }

        /// <summary>
        /// Indicates whether the texture had been loaded into memory with no errors.
        /// </summary>
        bool IsOk { get; }

        /// <summary>
        /// Texture height
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Texture width
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Loads the texture data into memory
        /// </summary>
        /// <param name="texture">Texture which contains the color data</param>
        void LoadTexture(Texture2D texture);

        /// <summary>
        /// Provides access to specified pixel
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>Color data of the pixel</returns>
        Color this[int x, int y] { get; }

    }
}
