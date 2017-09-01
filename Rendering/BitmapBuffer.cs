using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Xna.Framework;

namespace Rendering
{
    public class BitmapBuffer
    {
        public Color[] BufferData { get; private set; }

        public int Height { get; private set; }
        public int Width { get; private set; }

        public Color this[int x, int y]
        {
            get { return BufferData[y * Width + x]; }
            set { BufferData[y * Width + x] = value; }
        }

        public BitmapBuffer(int width, int height)
        {
            BufferData = new Color[height * width];
            Width = width;
            Height = height;
        }

        public void Resize(int width, int height)
        {
            BufferData = new Color[height * width];
            Width = width;
            Height = height;
        }
    }
}
