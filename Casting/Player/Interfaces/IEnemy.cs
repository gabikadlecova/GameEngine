using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.Environment.Interfaces;
using Casting.RayCasting.Interfaces;

namespace Casting.Player.Interfaces
{
    public interface IEnemy : IPerson, ICrossable
    {
        int TypeId { get; }

        int Width { get; }

        ITextureWrapper Texture { get; }
    }
}
