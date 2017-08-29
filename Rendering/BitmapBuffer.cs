using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering
{
    class BitmapBuffer
    {
        public int[] BufferData { get; private set; }

        public int Height { get; set; }
        public int Width { get; set; }

        public int this[int x, int y]
        {
            get { return BufferData[y * Width + x]; }
            set { BufferData[y * Width + x] = value; }
        }

        public BitmapBuffer(int width, int height)
        {
            BufferData = new int[height * width];
            Width = width;
            Height = height;
        }
    }
}
