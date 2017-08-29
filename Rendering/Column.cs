using System;
using System.Diagnostics;
using System.Drawing;
using Casting.Environment.Interfaces;
using Casting.RayCasting;
using Casting.RayCasting.Interfaces;
using Rendering.Interfaces;

namespace Rendering
{
    public class Column : IColumn
    {
        public int[] Pixels { get; set; }
        public int ColumnNr { get; }

        public Column(int columnNr, int height)
        {
            Pixels = new int[height];
            ColumnNr = columnNr;
        }
        public void SetPixels(IRay rayFrom)
        {
            for (int i = rayFrom.WallsCrossed.Count - 1; i >= 0; i--)
            {
                //todo check beginnings
                var item = rayFrom.WallsCrossed[i];

                int line = (int)(item.Element.HeightTotal / item.Distance);

                int begin = Pixels.Length / 2 - line / 2;

                begin = begin < 0 ? 0 : begin;

                int altColor = 0;
                ITexture texture = null;
                double heightRatio = 1;
                bool useTexture = false;
                
                try
                {
                    //todo major code cleanup
                    //todo do a texture bool and set it elsewhere for readability
                    switch (item.Side)
                    {
                        case Side.SideX:
                            altColor = item.Element.TextureX.AltColor.ToArgb();
                            texture = item.Element.TextureX;
                            
                            break;

                        case Side.SideY:
                            altColor = item.Element.TextureY.AltColor.ToArgb();
                            texture = item.Element.TextureY;
                            
                            break;

                        default:
                            altColor = item.Element.TextureX.AltColor.ToArgb();
                            break;
                    }
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
                        Pixels[begin + pixelNo] = texture[bitmapXCoor, bitmapPixelNo];
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
