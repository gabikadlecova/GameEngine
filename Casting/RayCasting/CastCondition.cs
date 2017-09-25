using Casting.RayCasting.Interfaces;

namespace Casting.RayCasting
{
    /// <summary>
    /// Limits raycasting distance or the number of object crossed with a ray
    /// </summary>
    public class CastCondition : ICastCondition
    {
        
        /// <summary>
        /// Determines whether the condition had been met yet.
        /// </summary>
        public bool IsMet { get; set; }

        /// <summary>
        /// Maximum wall number that can be crossed with a ray
        /// </summary>
        protected int? MaxWalls { get; set; }

        /// <summary>
        /// Maximum distance that can a ray pass
        /// </summary>
        protected double? MaxDistance { get; set; }

        /// <summary>
        /// Minimum wall count that should be crossed even if the other conditions are met
        /// </summary>
        public int? MinWalls { get; }

        /// <summary>
        /// Current obstacle count
        /// </summary>
        protected int ObstacleCount;

        /// <summary>
        /// Current ray distance
        /// </summary>
        protected double CurrentDistance;

        /// <summary>
        /// Updates the obstacle count and distance
        /// </summary>
        /// <param name="distance">Distance of the last obstacle</param>
        public virtual void ObstacleCrossed(double distance)
        {
            ObstacleCount++;
            CurrentDistance = distance;
            
            //Has the maximum wall count been reached?
            if (MaxWalls != null)
            {
                IsMet = ObstacleCount >= MaxWalls;
            }

            //Has the maximum distance been passed?
            if (MaxDistance != null)
            {
                IsMet = CurrentDistance >= MaxDistance || IsMet;
            }

            //Has the ray crossed a sufficient number of obstacles?
            if (MinWalls != null)
            {
                IsMet = ObstacleCount > MinWalls && IsMet;
            }
        }

        /// <summary>
        /// Indicates that the ray has reached the map border
        /// </summary>
        public void MapRangeCrossed()
        {
            ObstacleCount++;
            IsMet = true;
        }

        /// <summary>
        /// Resets the condition so that it can be reused in another ray cast
        /// </summary>
        public void Reset()
        {
            IsMet = false;
            ObstacleCount = 0;
            CurrentDistance = 0;
        }

        /// <summary>
        /// Updates the current distance
        /// </summary>
        /// <param name="distance">Current distance of the ray</param>
        public void UpdateDistance(double distance)
        {
            CurrentDistance = distance;

            if (MaxDistance != null)
                IsMet = CurrentDistance >= MaxDistance;
        }

        /// <summary>
        /// Initializes a new instance of a cast condition with specified sub-conditions
        /// </summary>
        /// <param name="wallNumber">Maximum wall number</param>
        /// <param name="maxDistance">Maximum ray distance</param>
        /// <param name="minWalls">Minimum wall number</param>
        protected CastCondition(int? wallNumber, double? maxDistance, int? minWalls)
        {
            MaxWalls = wallNumber;
            MaxDistance = maxDistance;
            MinWalls = minWalls;
            IsMet = false;
            ObstacleCount = 0;
            CurrentDistance = 0;
        }

        /// <summary>
        /// Provides a new ray cast condition only with the maximum wall count limit
        /// </summary>
        /// <param name="wallNumber">Maximum wall count</param>
        /// <returns>A new instance of Cast condition</returns>
        public static CastCondition LimitWalls(int wallNumber)
        {
            return new CastCondition(wallNumber, null, null);
        }

        /// <summary>
        /// Limits the maximum ray cast distance. The raycasting ends only if the ray has crossed a minimum number of walls.
        /// </summary>
        /// <param name="distance">Maximum ray distance</param>
        /// <param name="minWallCount">Minimum wall count</param>
        /// <returns>A new instance of a cast condition</returns>
        public static CastCondition CastDistance(double distance, int minWallCount)
        {
            return new CastCondition(null, distance, minWallCount);
        }


    }
}
