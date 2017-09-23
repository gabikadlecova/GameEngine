using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.Environment;
using Casting.Environment.Interfaces;
using Casting.RayCasting;
using Microsoft.Xna.Framework;

namespace Casting.Player
{
    public class SpriteData
    {
        public SpriteData(string flyPicAdd, string hitPicAdd, int height, int width)
        {
            LivingPic = new TextureWrapper(flyPicAdd, Color.Transparent);
            DeadPic = new TextureWrapper(hitPicAdd, Color.Transparent);
            Height = height;
            Width = width;
        }

        
        public ITextureWrapper LivingPic { get; }
        public ITextureWrapper DeadPic { get; }
        
        public int Height { get; }
        public int Width { get; }

    }
}
