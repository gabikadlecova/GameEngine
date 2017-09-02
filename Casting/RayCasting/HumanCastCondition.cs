using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casting.RayCasting
{
    public class HumanCastCondition : CastCondition
    {
        protected HumanCastCondition(int? wallNumber, double? maxDistance) : base(wallNumber, maxDistance)
        {
        }

        public static HumanCastCondition Default()
        {
            return new HumanCastCondition(1, double.MaxValue);
        }

        public void ResetDistance(double distance)
        {
            MaxDistance = distance;
        }

        public override void ObstacleCrossed(double distance)
        {
            _obstacleCount++;
            _currentDistance = distance;

            //todo make raycaster intersection check methode in order to enable walldistance update (which will not be called in the wall raycaster methode)
            if (MaxDistance != null)
                IsMet = _currentDistance >= MaxDistance;

            if (MaxWalls != null)
            {
                if(!IsMet)
                    IsMet = _obstacleCount >= MaxWalls;
            }
        }
    }
}
