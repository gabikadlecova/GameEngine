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
        private double[] _distances;

        public Color[] Buffer { get; private set; }


        public ITextureWrapper Sky { get; }
        public ITextureWrapper Floor { get; }

        private readonly List<ElementDrawData> elementData = new List<ElementDrawData>();
        public int Width { get; private set; }
        public int Height { get; private set; }

        public BackgroundPainter(int width, int height, string skyPath, string floorPath)
        {
            Buffer = new Color[width * height];
            Width = width;
            Height = height;

            Sky = new TextureWrapper(skyPath, Color.Azure);
            Floor = new TextureWrapper(floorPath, Color.DarkGreen);
        }

        //ToDo check which side is x and which y (bitmap context)
        public void ChangeResolution(int width, int height)
        {
            Buffer = new Color[height * width];
            Width = width;
            Height = height;
            //InitializeDistances();
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

        private void InitializeDistances(int maxHeight)
        {
            _distances = new double[Height / 2];

            for (int i = 0; i < Height / 2; i++)
            {
                int y = Height / 2 + i;
                _distances[i] = Height / (2.0 * y - Height);
            }
        }

        private void SetPixels(Ray rayFrom, int columnNr, int maxHeight, Vector2 playerPos, Vector2 direction)
        {
            if (_distances == null)
            {
                InitializeDistances(maxHeight);
            }


            //clearing the previous data (needed for transparent WALLS)
            for (int i = 0; i < Height; i++)
            {
                Buffer[columnNr + Width * i] = Color.Transparent;
            }
            elementData.Clear();

            bool firstWall = true;
            int maxWallPoint = Height;
            int highestPoint = Height;
            int lastIndex = -1;

            var elements = rayFrom.ObjectsCrossed;
            for (int i = 0; i < elements.Count; i++)
            {
                var item = elements[i];
                int line = (int)(item.Element.Height / item.Distance);
                int maxLine = (int)(maxHeight / item.Distance);

                int begin = Height / 2 + maxLine / 2 - line;
                int end = begin + line;
                end = end < highestPoint ? end : highestPoint;

                if (item.IsNotTransparent)
                {
                    highestPoint = begin < highestPoint ? begin : highestPoint;
                    begin = begin == highestPoint ? begin : Height;
                }


                maxWallPoint = begin < maxWallPoint ? begin : maxWallPoint;

                if (begin < Height)
                {
                    if (item.IsNotTransparent && firstWall)
                    {
                        lastIndex = i;
                        firstWall = false;
                    }
                    
                    elementData.Add(new ElementDrawData(begin, end,line, i));
                }
            }
            lastIndex = lastIndex < 0 ? elements.Count - 1 : lastIndex;


            //filling the column with pixels of crossed objects
            for (int index = elementData.Count - 1; index >= 0; index--)
            {
                var itemData = elementData[index];
                var item = elements[itemData.Index];

                int line = itemData.FullLine;
                int begin = itemData.HighestPoint;
                int end = itemData.LowestPoint;


                double heightRatio = 1;
                
                ITextureWrapper texture = item.Element.GetTexture(item.Side);
                Color altColor = texture.AltColor;

                if (texture.IsOk)
                {
                    heightRatio = (double)texture.Height / line;
                }

                int pixelNo = begin < 0 ? -begin : 0;
                int bitmapXCoor = (int)(item.TextureXRatio * texture.Width);



                while (/*pixelNo < line &&*/ begin + pixelNo < end)
                {

                    if (texture.IsOk)
                    {
                        int bitmapPixelNo = (int)(heightRatio * pixelNo);
                        Color nextPix = texture[bitmapPixelNo, bitmapXCoor];
                        if (nextPix != Color.Transparent)
                            Buffer[columnNr + Width * (begin + pixelNo)] = nextPix;
                    }
                    else
                    {
                        Buffer[columnNr + Width * (begin + pixelNo)] = altColor;
                    }


                    pixelNo++;
                }

                if (itemData.Index == lastIndex)
                {

                    for (int j = Height / 2; j < Height; j++)
                    {
                        double currentDist = _distances[j - Height / 2];
                        double weight = currentDist / item.Distance;

                        double floorX = weight * item.ElementPos.X + (1 - weight) * playerPos.X;
                        double floorY = weight * item.ElementPos.Y + (1 - weight) * playerPos.Y;


                        if (j >= begin + pixelNo)
                        {
                            int texX = (int)(floorY * Floor.Height) % Floor.Height;
                            int texY = (int)(floorX * Floor.Width) % Floor.Width;

                            texX = texX >= 0 ? texX : texX + Floor.Height;
                            texY = texY >= 0 ? texY : texY + Floor.Width;

                            Buffer[columnNr + j * Width] = Floor[texX, texY];
                        }

                        if (Height - j < maxWallPoint)
                        {

                            int texX = (int)(floorY * Sky.Height) % Sky.Height;
                            int texY = (int)(floorX * Sky.Width) % Sky.Width;

                            texX = texX >= 0 ? texX : texX + Sky.Height;
                            texY = texY >= 0 ? texY : texY + Sky.Width;

                            Buffer[columnNr + Width * (Height - j)] = Sky[texX, texY];
                        }
                    }

                }

            }




        }

        private struct ElementDrawData
        {
            public ElementDrawData(int highestPoint, int lowestPoint, int fullLine, int index)
            {
                HighestPoint = highestPoint;
                LowestPoint = lowestPoint;
                Index = index;
                FullLine = fullLine;
            }

            public int HighestPoint;
            public int LowestPoint;
            public int FullLine;
            public int Index;
        }

    }
}
