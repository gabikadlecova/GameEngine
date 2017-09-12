using System;
using System.Diagnostics;
using Casting.Environment.Interfaces;
using Casting.RayCasting;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;
using Rendering.Interfaces;

namespace Rendering
{
    public class Column : IColumn
    {
        public Color[] Pixels { get; set; }
        public int ColumnNr { get; }

        public Column(int columnNr, int height)
        {
            Pixels = new Color[height];
            ColumnNr = columnNr;
        }
        public void SetPixels(IRay rayFrom, int maxHeight)
        {
            for (int i = rayFrom.ObjectsCrossed.Count - 1; i >= 0; i--)
            {
                //todo check beginnings
                var item = rayFrom.ObjectsCrossed[i];

                int line = (int)(item.Element.Height / item.Distance);
                int maxLine = (int) (maxHeight / item.Distance);

                int begin = Pixels.Length / 2 + maxLine / 2 - line;

                begin = begin < 0 ? 0 : begin;

                Color altColor = Color.DarkRed;
                ITextureWrapper texture = null;
                double heightRatio = 1;
                bool useTexture = false;
                
                try
                {
                    texture = item.Element.GetTexture(item.Side);
                    altColor = texture.AltColor;
                }
                catch (Exception e)
                {
                    //todo make a texture isOk
                    Debug.WriteLine(e);
                }

                if (texture != null && texture.IsOk)
                {
                    heightRatio = (double)texture.Height / line;
                    useTexture = true;
                }
                

                int pixelNo = 0;
                while (pixelNo < line && begin + pixelNo < Pixels.Length)
                {
                    if (useTexture)
                    {
                        int bitmapPixelNo = (int) (heightRatio * pixelNo);
                        int bitmapXCoor = (int)(item.TextureXRatio * texture.Width);
                        Color nextPix = texture[bitmapPixelNo, bitmapXCoor];
                        if(nextPix.A != 0)
                            Pixels[begin + pixelNo] = nextPix;
                    }
                    else
                    {
                        Pixels[begin + pixelNo] = altColor;
                    }
                    pixelNo++;
                }


            }

        }
    }
}
