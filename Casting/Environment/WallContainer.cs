using System;
using System.Collections;
using System.Collections.Generic;
using Casting.Environment.Exceptions;
using Casting.Environment.Interfaces;

namespace Casting.Environment
{
    public class WallContainer : IWallContainer
    {
        private readonly Dictionary<int, IWall> _wallTypes;

        public WallContainer()
        {
            _wallTypes = new Dictionary<int, IWall>();
        }

        public IWall this[int i]
        {
            get
            {
                return _wallTypes[i];
            }
            set
            {
                if (_wallTypes.ContainsKey(i))
                    throw new WallAssignmentException("Wall type is already defined for this index.");
                _wallTypes[i] = value;
            }
        }


        public IEnumerator<IWall> GetEnumerator()
        {
            return _wallTypes.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
