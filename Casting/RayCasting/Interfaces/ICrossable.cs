using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.Environment.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace Casting.RayCasting.Interfaces
{
    public interface ICrossable
    {
        int Height { get; }

        ITextureWrapper GetTexture(Side side);

    }
}
