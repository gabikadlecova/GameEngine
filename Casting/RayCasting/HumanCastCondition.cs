using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casting.RayCasting
{
    /// <summary>
    /// Represents a cast condition that could be used for movement
    /// </summary>
    public class HumanCastCondition : CastCondition
    {
        /// <summary>
        /// Initializes a new instance of the human movement condition
        /// </summary>
        /// <param name="wallNumber">Maximum walls a human can cross at a time</param>
        /// <param name="maxDistance">Maximum cast distance</param>
        protected HumanCastCondition(int? wallNumber, double? maxDistance) : base(wallNumber, maxDistance, null)
        {
        }

        /// <summary>
        /// Provides the default human or object movement condition.
        /// </summary>
        /// <returns>Default movement condition</returns>
        public static HumanCastCondition Default()
        {
            return new HumanCastCondition(1, double.MaxValue);
        }

        /// <summary>
        /// Resets the movement condition and sets a new maximum distance
        /// </summary>
        /// <param name="maxDistance">Maximum distance for raycasting</param>
        public void Reset(double maxDistance)
        {
            MaxDistance = maxDistance;
            Reset();
        }

        /// <summary>
        /// This method should be called when an obstacle is crossed. Sub-conditions are updated.
        /// </summary>
        /// <param name="distance">Current distance</param>
        public override void ObstacleCrossed(double distance)
        {
            ObstacleCount++;
            CurrentDistance = distance;
            
            //has the distance limit been reached?
            if (MaxDistance != null)
                IsMet = CurrentDistance >= MaxDistance;

            //only distance is limited
            if (MaxWalls == null) return;

            if (!IsMet)
                IsMet = ObstacleCount >= MaxWalls;
        }
    }
}
