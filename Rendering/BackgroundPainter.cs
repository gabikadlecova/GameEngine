using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Casting.RayCasting;
using Casting.RayCasting.Interfaces;
using Rendering.Interfaces;

namespace Rendering
{
    public class BackgroundPainter : IDisposable
    {

        private Bitmap _bitmap;
        private BitmapBuffer _buffer;


        public BackgroundPainter(int width, int height)
        {
            _bitmap = new Bitmap(width, height);
            _buffer = new BitmapBuffer(width, height);
        }

        public void UpdateBuffer(IRay ray, int columnNr)
        {
            
                Stopwatch watch = Stopwatch.StartNew();
                //todo copy column data
                IColumn column = new Column(columnNr, _buffer.Height);
                column.SetPixels(ray);

                for (int i = 0; i < _buffer.Height; i++)
                {
                    _buffer[columnNr, i] = column.Pixels[i];
                }

                watch.Stop();
                
        }

        public void RenderBitmap(Graphics g)
        {
            //todo which pixel format
            //todo think of what is quicker

            int width = _buffer.Width;
            int height = _buffer.Height;

            var rectangle = new Rectangle(0,0,width, height);
            BitmapData data = _bitmap.LockBits(rectangle, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            IntPtr ptr = data.Scan0;
            

            
            Marshal.Copy(_buffer.BufferData, 0, ptr, _buffer.BufferData.Length);
            _bitmap.UnlockBits(data);

            Stopwatch secWatch = Stopwatch.StartNew();

            //todo should be done differently
            g.DrawImage(_bitmap, new Point(0,0));
            secWatch.Stop();
        }

        //ToDo check which side is x and which y (bitmap context)
        public void ChangeResolution(int width, int height)
        {
            _buffer = new BitmapBuffer(width, height);
            _bitmap = new Bitmap(_bitmap, new Size(width, height));
        }



        public void Dispose()
        {
            _bitmap.Dispose();
        }
    }
}
