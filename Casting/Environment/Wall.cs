using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.Environment.Interfaces;

namespace Casting.Environment
{
    class Wall : IWall
    {
        public Wall(string textureX, string textureY, Color altX, Color altY, int height)
        {
            TextureX = new Texture(textureX, altX);
            TextureY = new Texture(textureY, altY);
            HeightTotal = height;
        }
        
        public int HeightTotal { get; }
        public ITexture TextureX { get; }
        public ITexture TextureY { get; }
    }
}
