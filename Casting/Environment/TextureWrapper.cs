using Casting.Environment.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Casting.Environment
{
    public class TextureWrapper : ITextureWrapper
    {
        private Color[,] _buffer;


        public TextureWrapper(string picAddress, Color altColor)
        {
            PicAddress = picAddress;
            AltColor = altColor;
        }

        public bool IsOk
        {
            get { return !ReferenceEquals(_buffer, null); }
        }

        public int Height
        {
            get { return _buffer.GetLength(0); }
        }

        public int Width
        {
            get { return _buffer.GetLength(1); }
        }

        /*public void LoadBitmap()
        {
            try
            {
                using (Bitmap bitmap = (Bitmap)Image.FromFile(PicAddress))
                {
                    BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    int[] line = new int[data.Width * data.Height];
                    Marshal.Copy(data.Scan0,line, 0, data.Width * data.Height);

                    _buffer = new int[data.Width, data.Height];

                    int startIndex = 0;
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        {
                            _buffer[j, i] = line[startIndex];
                            startIndex++;
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception("Bitmap could not be loaded (see inner exception for more information).", e);
            }
        }*/

        public string PicAddress { get; }
        public Color AltColor { get; }

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

        public Color this[int x, int y]
        {
            get { return _buffer[x, y]; }
        }
    }
}
