using Casting.RayCasting.Interfaces;

namespace Rendering.Interfaces
{
    interface IColumn
    {
        int[] Pixels { get; set; }

        int ColumnNr { get; }

        void SetPixels(IRay rayFrom);
    }
}
