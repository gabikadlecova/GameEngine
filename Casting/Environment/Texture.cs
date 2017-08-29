using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Casting.Environment.Interfaces;

namespace Casting.Environment
{
    class Texture : ITexture
    {
        private int[,] _bitmap;


        public Texture(string picAddress, Color altColor)
        {
            PicAddress = picAddress;
            AltColor = altColor;
        }

        public bool IsOk
        {
            get { return !ReferenceEquals(_bitmap, null); }
        }

        public int Height
        {
            get { return _bitmap.GetLength(1); }
        }

        public int Width
        {
            get { return _bitmap.GetLength(0); }
        }

        public void LoadBitmap()
        {
            try
            {
                using (Bitmap bitmap = (Bitmap)Image.FromFile(PicAddress))
                {
                    BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    int[] line = new int[data.Width * data.Height];
                    Marshal.Copy(data.Scan0,line, 0, data.Width * data.Height);

                    _bitmap = new int[data.Width, data.Height];

                    int startIndex = 0;
                    for (int i = 0; i < data.Height; i++)
                    {
                        for (int j = 0; j < data.Width; j++)
                        {
                            _bitmap[j, i] = line[startIndex];
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
        }

        public string PicAddress { get; }
        public Color AltColor { get; }

        public int this[int x, int y]
        {
            get { return _bitmap[x, y]; }
        }
    }
}
