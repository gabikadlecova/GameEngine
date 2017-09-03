using System;
using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace Casting.Environment.Interfaces
{
    public interface IWall : ICrossable
    {
        ITextureWrapper TextureX { get; }

        ITextureWrapper TextureY { get; }
    }
}
