using Casting.Environment.Interfaces;
using Microsoft.Xna.Framework;

namespace Casting.Environment
{
    class Wall : IWall
    {
        public Wall(string textureX, string textureY, Color altX, Color altY, int height)
        {
            TextureX = new TextureWrapper(textureX, altX);
            TextureY = new TextureWrapper(textureY, altY);
            HeightTotal = height;
        }

        public int HeightTotal { get; }
        public ITextureWrapper TextureX { get; }
        public ITextureWrapper TextureY { get; }
    }
}
