﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casting.RayCasting;
using Microsoft.Xna.Framework;

namespace Casting.Player.Interfaces
{
    public interface IPerson
    {
       int HitPoints { get; set; }
       bool IsKilled { get; }

    }
}
