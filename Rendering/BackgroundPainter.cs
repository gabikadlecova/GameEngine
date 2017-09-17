using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Casting.RayCasting.Interfaces;
using System.Windows.Media.Imaging;
using Casting.Environment.Interfaces;
using Microsoft.Xna.Framework;
using Ray = Casting.RayCasting.Ray;

namespace Rendering
{
    public class BackgroundPainter
    {

        public BitmapBuffer Buffer { get; }
        

        public BackgroundPainter(int width, int height)
        {
            Buffer = new BitmapBuffer(width, height);
        }

        //ToDo check which side is x and which y (bitmap context)
        public void ChangeResolution(int width, int height)
        {
            Buffer.Resize(width, height);
        }


        public void UpdateBuffer(List<Ray> rays, int wallMaxHeight)
        {
            for (int i = 0; i < rays.Count; i++)
            {
                SetPixels(rays[i], i, wallMaxHeight);
            }
        }

        public void UpdateBuffer(Ray ray, int columnNr, int wallMaxHeight)
        {
            SetPixels(ray, columnNr, wallMaxHeight);
        }

        private void SetPixels(Ray rayFrom, int columnNr, int maxHeight)
        {
            //clearing the previous data
            for (int i = 0; i < Buffer.Height; i++)
            {
                Buffer[columnNr, i] = Color.Transparent;
            }

            //filling the column with pixels of crossed objects
            for (int i = rayFrom.ObjectsCrossed.Count - 1; i >= 0; i--)
            {
                //todo check beginnings
                var item = rayFrom.ObjectsCrossed[i];

                int line = (int)(item.Element.Height / item.Distance);
                int maxLine = (int)(maxHeight / item.Distance);

                int begin = Buffer.Height / 2 + maxLine / 2 - line;

                begin = begin < 0 ? 0 : begin;

                double heightRatio = 1;
                bool useTexture = false;


                ITextureWrapper texture = item.Element.GetTexture(item.Side);
                Color altColor = texture.AltColor;

                if (texture.IsOk)
                {
                    heightRatio = (double)texture.Height / line;
                    useTexture = true;
                }


                int pixelNo = 0;
                int bitmapXCoor = (int)(item.TextureXRatio * texture.Width);

                while (pixelNo < line && begin + pixelNo < Buffer.Height)
                {
                    if (useTexture)
                    {
                        int bitmapPixelNo = (int)(heightRatio * pixelNo);
                        Color nextPix = texture[bitmapPixelNo, bitmapXCoor];
                        if (nextPix != Color.Transparent)
                            Buffer[columnNr, begin + pixelNo] = nextPix;
                    }
                    else
                    {
                        Buffer[columnNr, begin + pixelNo] = altColor;
                    }
                    pixelNo++;
                }


            }

        }

    }
}
