using Casting.RayCasting.Interfaces;
using System.Collections.Generic;

namespace Casting.Environment.Interfaces
{
    /// <summary>
    /// This class can store indexed ICrossable elements
    /// </summary>
    /// <typeparam name="TObj">Type derived from ICrossable</typeparam>
    public interface IContainer<TObj> : IEnumerable<TObj> where TObj : ICrossable
    {
        /// <summary>
        /// Provides access to ICrossable elements stored under a specified index
        /// </summary>
        /// <param name="i">Element index</param>
        /// <returns>Element stored under the specified index</returns>
        TObj this[int i] { get; set; }
        /// <summary>
        /// Maximum element height
        /// </summary>
        int MaxHeight { get; }
    }
}
