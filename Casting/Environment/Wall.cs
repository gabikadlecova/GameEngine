﻿using System;
using System.Collections.Generic;
using Casting.Environment.Interfaces;
using Casting.RayCasting;
using Microsoft.Xna.Framework;

namespace Casting.Environment
{
    public class Wall : IWall
    {
        public Wall(string textureX, string textureY, Color altX, Color altY, int height)
        {
            Textures = new List<ITextureWrapper>
            {
                new TextureWrapper(textureX, altX),
                new TextureWrapper(textureY, altY)
            };
            Height = height;
        }

        public int Height { get; }
        public int Width { get { return 0; } }

        public ITextureWrapper GetTexture(Side side)
        {
            switch (side)
            {
                case Side.SideX:
                    return TextureX;
                case Side.SideY:
                    return TextureY;
                default:

                    throw new ArgumentException("This side is not defined for this object.");
            }
        }

        public List<ITextureWrapper> Textures { get; }
        public ITextureWrapper TextureX { get { return Textures[0]; } }
        public ITextureWrapper TextureY { get { return Textures[1]; } }
    }
}
