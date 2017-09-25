using Casting.Environment.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Casting.Environment
{
    /// <summary>
    /// Provides direct access to loaded texture pixel data
    /// </summary>
    public class TextureWrapper : ITextureWrapper
    {
        private Color[,] _buffer;

        /// <summary>
        /// Initializes a new wrapper instance which can be later used to load the texture into memory
        /// </summary>
        /// <param name="picAddress">Texture path</param>
        /// <param name="altColor">Alternative pixel color</param>
        public TextureWrapper(string picAddress, Color altColor)
        {
            PicAddress = picAddress;
            AltColor = altColor;
        }

        /// <summary>
        /// Is true if the texture had been loaded correctly
        /// </summary>
        public bool IsOk
        {
            get { return !ReferenceEquals(_buffer, null); }
        }

        /// <summary>
        /// Texture height
        /// </summary>
        public int Height
        {
            get { return _buffer.GetLength(0); }
        }

        /// <summary>
        /// Texture width
        /// </summary>
        public int Width
        {
            get { return _buffer.GetLength(1); }
        }

        /// <summary>
        /// Texture path
        /// </summary>
        public string PicAddress { get; }

        /// <summary>
        /// Can be used as alternative pixel color if the texture had not been loaded
        /// </summary>
        public Color AltColor { get; }

        /// <summary>
        /// Loads the texture into memory
        /// </summary>
        /// <param name="texture">Texture which contains pixel color data</param>
        public void LoadTexture(Texture2D texture)
        {
            _buffer = new Color[texture.Height, texture.Width];
            Color[] colorTemp = new Color[texture.Width * texture.Height];
            texture.GetData(colorTemp);

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    _buffer[i, j] = colorTemp[i * Width + j];
                }
            }
        }

        /// <summary>
        /// Provides access to specified pixel color
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns></returns>
        public Color this[int x, int y]
        {
            get { return _buffer[x, y]; }
        }
    }
}
