using Casting.Environment.Interfaces;
using Casting.RayCasting.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Input.Exceptions;

namespace Casting.Environment
{
    /// <summary>
    /// Implementation of IContainer for ICrossable objects
    /// </summary>
    /// <typeparam name="TObj">ICrossable objects</typeparam>
    public class Container<TObj> : IContainer<TObj> where TObj : ICrossable
    {
        /// <summary>
        /// Indexed objects
        /// </summary>
        private readonly Dictionary<int, TObj> _objects;

        /// <summary>
        /// Initializes a new Container of ICrossables
        /// </summary>
        public Container()
        {
            _objects = new Dictionary<int, TObj>();
            MaxHeight = 0;
        }

        /// <summary>
        /// Gets the specified ICrossable element by index
        /// </summary>
        /// <param name="i">Element index</param>
        /// <returns>ICrossable element stored under a specified index</returns>
        public TObj this[int i]
        {
            get
            {
                return _objects[i];
            }
            set
            {
                if (_objects.ContainsKey(i))
                    throw new ContainerAssignmentException("An object is already defined for this index.");
                _objects[i] = value;
                MaxHeight = value.Height > MaxHeight ? value.Height : MaxHeight;
            }
        }

        /// <summary>
        /// Maximum height of all ICrossable elements in this container
        /// </summary>
        public int MaxHeight { get; private set; }

        /// <summary>
        /// Gets the default enumerator
        /// </summary>
        /// <returns>Default enumerator</returns>
        public IEnumerator<TObj> GetEnumerator()
        {
            return _objects.Values.GetEnumerator();
        }

        /// <summary>
        /// Gets the default enumerator
        /// </summary>
        /// <returns>Default enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
