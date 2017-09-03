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
using System.Windows.Media.Imaging;

namespace Rendering
{
    public class BackgroundPainter
    {

        public BitmapBuffer Buffer { get; }


        public BackgroundPainter(int width, int height)
        {
            Buffer = new BitmapBuffer(width, height);
        }

        public void UpdateBuffer(IRay ray, int columnNr, int wallMaxHeight)
        {
            
                Stopwatch watch = Stopwatch.StartNew();
                //todo copy column data
                IColumn column = new Column(columnNr, Buffer.Height);
                column.SetPixels(ray, wallMaxHeight);

                for (int i = 0; i < Buffer.Height; i++)
                {
                    Buffer[columnNr, i] = column.Pixels[i];
                }

                watch.Stop();
                
        }

        /*public void RenderBitmap(Graphics g)
        {
            //todo which pixel format
            //todo think of what is quicker

            int width = Buffer.Width;
            int height = Buffer.Height;

            var rectangle = new Rectangle(0,0,width, height);
            BitmapData data = _bitmap.LockBits(rectangle, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            IntPtr ptr = data.Scan0;
            

            
            //Marshal.Copy(Buffer.BufferData, 0, ptr, Buffer.BufferData.Length);
            _bitmap.UnlockBits(data);

            Stopwatch secWatch = Stopwatch.StartNew();

            //todo should be done differently
            g.DrawImage(_bitmap, new Point(0,0));
            secWatch.Stop();
        }*/

        //ToDo check which side is x and which y (bitmap context)
        public void ChangeResolution(int width, int height)
        {
            Buffer.Resize(width, height);
            //_bitmap = new Bitmap(_bitmap, new Size(width, height));
        }



       /* public void Dispose()
        {
            _bitmap.Dispose();
        }*/
    }
}
