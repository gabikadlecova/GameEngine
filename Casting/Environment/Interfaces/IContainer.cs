using System;
using System.Collections;
using System.Collections.Generic;
using Casting.RayCasting.Interfaces;

namespace Casting.Environment.Interfaces
{
    public interface IContainer<TObj> : IEnumerable<TObj> where TObj : ICrossable
    {
        TObj this[int i] { get; set; }

        int MaxHeight { get; }
    }
}
