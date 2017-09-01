using System;
using Microsoft.Xna.Framework.Graphics;

namespace Casting.Environment.Interfaces
{
    public interface IWall
    {
        int HeightTotal { get; }

        ITextureWrapper TextureX { get; }

        ITextureWrapper TextureY { get; }
    }
}
