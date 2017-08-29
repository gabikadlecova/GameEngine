using System;

namespace Casting.Environment.Interfaces
{
    public interface IWall
    {
        int HeightTotal { get; }

        ITexture TextureX { get; }

        ITexture TextureY { get; }
    }
}
