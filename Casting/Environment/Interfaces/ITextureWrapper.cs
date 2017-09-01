using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Casting.Environment.Interfaces
{
    public interface ITextureWrapper
    {
        string PicAddress { get; }

        Color AltColor { get; }

        bool IsOk { get; }

        int Height { get; }

        int Width { get; }

        void LoadTexture(Texture2D texture);

        Color this[int x, int y] { get; }

    }
}
