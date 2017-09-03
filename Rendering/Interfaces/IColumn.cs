using Casting.RayCasting.Interfaces;
using Microsoft.Xna.Framework;

namespace Rendering.Interfaces
{
    interface IColumn
    {
        Color[] Pixels { get; set; }

        int ColumnNr { get; }

        void SetPixels(IRay rayFrom, int maxHeight);
    }
}
