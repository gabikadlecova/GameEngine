using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casting.RayCasting
{
    public class HumanCastCondition : CastCondition
    {
        //todo minwalls?
        protected HumanCastCondition(int? wallNumber, double? maxDistance) : base(wallNumber, maxDistance, null)
        {
        }

        //todo specify how the condition has been met!!

        public static HumanCastCondition Default()
        {
            return new HumanCastCondition(1, double.MaxValue);
        }

        public void ResetDistance(double maxDistance)
        {
            MaxDistance = maxDistance;
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
