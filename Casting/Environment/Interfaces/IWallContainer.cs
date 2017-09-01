using System;
using System.Collections;
using System.Collections.Generic;

namespace Casting.Environment.Interfaces
{
    public interface IWallContainer : IEnumerable<IWall>
    {
        IWall this[int i] { get; set; }
    }
}
