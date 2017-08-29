using System;

namespace Casting.Environment.Interfaces
{
    public interface IWallContainer
    {
        IWall this[int i] { get; set; }
    }
}
