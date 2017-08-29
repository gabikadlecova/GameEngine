using System;
using System.Drawing;

namespace Casting.Environment.Interfaces
{
    public interface ITexture
    {
        string PicAddress { get; }

        Color AltColor { get; }

        bool IsOk { get; }

        int Height { get; }

        int Width { get; }
        void LoadBitmap();

        int this[int x, int y] { get; }

    }
}
