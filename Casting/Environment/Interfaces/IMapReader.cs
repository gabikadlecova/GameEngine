using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casting.Environment.Interfaces
{
    public interface IMapReader
    {
        IMap ReadMap(string filePath);

        IWallContainer ReadWalls(string filePath);
    }
}
