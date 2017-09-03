using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Casting.Environment.Exceptions;
using Casting.Environment.Interfaces;
using Casting.RayCasting.Interfaces;

namespace Casting.Environment
{
    public class Container<TObj> : IContainer<TObj> where TObj : ICrossable
    {
        private readonly Dictionary<int, TObj> _objects;

        public Container()
        {
            _objects = new Dictionary<int, TObj>();
            MaxHeight = 0;
        }

        public TObj this[int i]
        {
            get
            {
                return _objects[i];
            }
            set
            {
                if (_objects.ContainsKey(i))
                    throw new WallAssignmentException("An object is already defined for this index.");
                _objects[i] = value;
                MaxHeight = value.Height > MaxHeight ? value.Height : MaxHeight;
            }
        }

        public int MaxHeight { get; private set; }


        public IEnumerator<TObj> GetEnumerator()
        {
            return _objects.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
