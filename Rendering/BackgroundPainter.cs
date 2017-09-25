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
    /// <summary>
    /// Creates a picture of environment from the ray data created by raycasters.
    /// </summary>
    public class BackgroundPainter
    {
        private double[] _distances;
        /// <summary>
        /// Contains buffered raw pixel color data.
        /// </summary>
        public Color[] Buffer { get; private set; }

        /// <summary>
        /// Contains pixel data of the sky texture.
        /// </summary>
        public ITextureWrapper Sky { get; }
        /// <summary>
        /// Contains pixel data of the floor texture.
        /// </summary>
        public ITextureWrapper Floor { get; }

        /// <summary>
        /// Contains computed temporary data of elements to be drawn.
        /// </summary>
        private readonly List<ElementDrawData> _elementData = new List<ElementDrawData>();
        /// <summary>
        /// Output texture width.
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// Output texture height.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Initializes a new instance of the background painter.
        /// </summary>
        /// <param name="width">Width of the output texture.</param>
        /// <param name="height">Height of the output texture.</param>
        /// <param name="skyPath">Path to the sky texture.</param>
        /// <param name="floorPath">Path to the floor texture.</param>
        public BackgroundPainter(int width, int height, string skyPath, string floorPath)
        {
            Buffer = new Color[width * height];
            Width = width;
            Height = height;

            Sky = new TextureWrapper(skyPath, Color.Azure);
            Floor = new TextureWrapper(floorPath, Color.DarkGreen);
        }

        /// <summary>
        /// Changes the output texture parameters and the buffer size.
        /// </summary>
        /// <param name="width">New texture width.</param>
        /// <param name="height">New texture height.</param>
        public void ChangeResolution(int width, int height)
        {
            Buffer = new Color[height * width];
            Width = width;
            Height = height;
            InitializeDistances();
        }

        /// <summary>
        /// Updates all columns of the texture, sets all pixel data.
        /// </summary>
        /// <param name="rays">List of rays for every column.</param>
        /// <param name="wallMaxHeight">Maximum wall height.</param>
        /// <param name="position">Current position on the map.</param>
        public void UpdateBuffer(List<Ray> rays, int wallMaxHeight, Vector2 position)
        {
            for (int i = 0; i < rays.Count; i++)
            {
                SetPixels(rays[i], i, wallMaxHeight, position);
            }
        }

        /// <summary>
        /// Updates the buffer with a single ray which corresponds to exactly one column.
        /// </summary>
        /// <param name="ray">Ray data</param>
        /// <param name="columnNr">Number of the column</param>
        /// <param name="wallMaxHeight">Maximum wall height</param>
        /// <param name="position">Current position on the map</param>
        public void UpdateBuffer(Ray ray, int columnNr, int wallMaxHeight, Vector2 position)
        {
            SetPixels(ray, columnNr, wallMaxHeight, position);
        }

        /// <summary>
        /// Initializes the pixel distance buffer which is used for floor and sky casting
        /// </summary>
        private void InitializeDistances()
        {
            _distances = new double[Height / 2];

            for (int i = 0; i < Height / 2; i++)
            {
                int y = Height / 2 + i;
                //formula for distances – middle point is infinite, close to the player is 1
                _distances[i] = Height / (2.0 * y - Height);
            }
        }

        /// <summary>
        /// Sets pixels of a single column using ray data.
        /// </summary>
        /// <param name="rayFrom">Ray data of crossed objects.</param>
        /// <param name="columnNr">Number of the column.</param>
        /// <param name="maxHeight">Maximum wall height.</param>
        /// <param name="playerPos">Player position</param>
        private void SetPixels(Ray rayFrom, int columnNr, int maxHeight, Vector2 playerPos)
        {
            //needed for floor and roof casting
            if (_distances == null)
            {
                InitializeDistances();
            }


            //clearing the previous data (needed for transparent WALLS)
            for (int i = 0; i < Height; i++)
            {
                Buffer[columnNr + Width * i] = Color.Transparent;
            }
            _elementData.Clear();

            bool firstWall = true;
            //sky end in a column (highest wall point)
            int maxWallPoint = Height;
            //highest wall point from all non-transparent walls before
            int highestPoint = Height;
            //index of the last non-transparent wall drawn (the nearest one)
            int lastIndex = -1;

            //first check which walls should be drawn and which not. Also, determine how many wall pixels are actually visible
            var elements = rayFrom.ObjectsCrossed;
            for (int i = 0; i < elements.Count; i++)
            {
                var item = elements[i];
                //full line of pixels size
                int line = (int)(item.Element.Height / item.Distance);
                //line size for walls of maximum height (we need this to avoid floating smaller walls)
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
                    
                    _elementData.Add(new ElementDrawData(begin, end,line, i));
                }
            }
            lastIndex = lastIndex < 0 ? elements.Count - 1 : lastIndex;


            //filling the column with pixels of crossed objects
            for (int index = _elementData.Count - 1; index >= 0; index--)
            {
                var itemData = _elementData[index];
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
