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
using Casting.Environment;
using Casting.Environment.Interfaces;
using Casting.RayCasting;
using Microsoft.Xna.Framework;
using Ray = Casting.RayCasting.Ray;

namespace Rendering
{
    public class BackgroundPainter
    {
        private const float SightDistance = 100;
        private double[] Distances;

        public BitmapBuffer Buffer { get; }

        public ITextureWrapper Sky { get; }
        public ITextureWrapper Floor { get; }

        public BackgroundPainter(int width, int height, string skyPath, string floorPath)
        {
            Buffer = new BitmapBuffer(width, height);
            Sky = new TextureWrapper(skyPath, Color.Azure);
            Floor = new TextureWrapper(floorPath, Color.DarkGreen);
        }

        //ToDo check which side is x and which y (bitmap context)
        public void ChangeResolution(int width, int height)
        {
            Buffer.Resize(width, height);
            InitializeDistances();
        }

        

        public void UpdateBuffer(List<Ray> rays, int wallMaxHeight, Vector2 position, Vector2 direction)
        {
            for (int i = 0; i < rays.Count; i++)
            {
                SetPixels(rays[i], i, wallMaxHeight, position, direction);
            }
        }

        public void UpdateBuffer(Ray ray, int columnNr, int wallMaxHeight, Vector2 position, Vector2 direction)
        {
            SetPixels(ray, columnNr, wallMaxHeight, position, direction);
        }

        private void InitializeDistances()
        {
            int height = Buffer.Height;
            Distances = new double[height / 2];
            for (int i = 0; i < height / 2; i++)
            {
                int y = height / 2 + 1 + i;
                Distances[i] = height / (2.0 * y - height);
            }
        }

        private void SetPixels(Ray rayFrom, int columnNr, int maxHeight, Vector2 playerPos, Vector2 direction)
        {
            if (Distances == null)
            {
                InitializeDistances();
            }

            
            //clearing the previous data (needed for transparent WALLS)
            for (int i = 0; i < Buffer.Height; i++)
            {
                Buffer[columnNr, i] = Color.Transparent;
            }
            

            int lastWallNo = -1;

            for (int i = 0; i < rayFrom.ObjectsCrossed.Count; i++)
            {
                if (rayFrom.ObjectsCrossed[i].IsNotTransparent)
                {
                    lastWallNo = i;
                    break;
                }
            }

            int height = Buffer.Height;

            int highestPoint = height;

            //filling the column with pixels of crossed objects
            for (int i = rayFrom.ObjectsCrossed.Count - 1; i >= 0; i--)
            {
                var item = rayFrom.ObjectsCrossed[i];

                int line = (int)(item.Element.Height / item.Distance);
                int maxLine = (int)(maxHeight / item.Distance);

                int begin = height / 2 + maxLine / 2 - line;
                highestPoint = begin < highestPoint ? begin : highestPoint;

                double heightRatio = 1;
                bool useTexture = false;


                ITextureWrapper texture = item.Element.GetTexture(item.Side);
                Color altColor = texture.AltColor;

                if (texture.IsOk)
                {
                    heightRatio = (double)texture.Height / line;
                    useTexture = true;
                }

                int pixelNo = begin < 0 ? -begin : 0;
                int bitmapXCoor = (int)(item.TextureXRatio * texture.Width);

                while (pixelNo < line && begin + pixelNo < height)
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











                if (i == lastWallNo)
                {

                    for (int j = height / 2; j < height; j++)
                    {
                        double currentDist = Distances[j - height / 2];
                        double weight = currentDist / item.Distance;

                        double floorX = weight * item.ElementPos.X + (1 - weight) * playerPos.X;
                        double floorY = weight * item.ElementPos.Y + (1 - weight) * playerPos.Y;


                        if (j >= begin + pixelNo)
                        {
                            int texX = (int)(floorY * Floor.Height) % Floor.Height;
                            int texY = (int)(floorX * Floor.Width) % Floor.Width;

                            texX = texX >= 0 ? texX : texX + Floor.Height;
                            texY = texY >= 0 ? texY : texY + Floor.Width;

                            Buffer[columnNr, j] = Floor[texX, texY];
                        }

                        if (height - j < highestPoint)
                        {

                            int texX = (int)(floorY * Sky.Height) % Sky.Height;
                            int texY = (int)(floorX * Sky.Width) % Sky.Width;

                            texX = texX >= 0 ? texX : texX + Sky.Height;
                            texY = texY >= 0 ? texY : texY + Sky.Width;

                            Buffer[columnNr, height - j] = Sky[texX, texY];
                        }
                    }

                }
                
            }

            

        }

    }
}
