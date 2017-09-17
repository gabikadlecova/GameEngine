using Casting.RayCasting.Interfaces;

namespace Casting.RayCasting
{
    public class CastCondition : ICastCondition
    {
        //ToDo check

        public bool IsMet { get; set; }

        protected int? MaxWalls { get; set; }

        protected double? MaxDistance { get; set; }

        public int? MinWalls { get; }

        protected int ObstacleCount;

        protected double CurrentDistance;

        public virtual void ObstacleCrossed(double distance)
        {
            ObstacleCount++;
            CurrentDistance = distance;

            if (MaxDistance != null)
                IsMet = CurrentDistance >= MaxDistance;

            if (MaxWalls != null)
            {
                IsMet = ObstacleCount >= MaxWalls;
            }

            if (MinWalls != null)
            {
                IsMet = ObstacleCount < MinWalls;
            }
        }
        public void Reset()
        {
            IsMet = false;
            ObstacleCount = 0;
            CurrentDistance = 0;
        }

        public void UpdateDistance(double distance)
        {
            CurrentDistance = distance;

            if (MaxDistance != null)
                IsMet = CurrentDistance >= MaxDistance;
        }

        protected CastCondition(int? wallNumber, double? maxDistance, int? minWalls)
        {
            MaxWalls = wallNumber;
            MaxDistance = maxDistance;
            MinWalls = minWalls;
            IsMet = false;
            ObstacleCount = 0;
            CurrentDistance = 0;
        }

        public static CastCondition LimitWalls(int wallNumber)
        {
            return new CastCondition(wallNumber, null, null);
        }

        public static CastCondition CastDistance(double distance, int minWallCount)
        {
            return new CastCondition(null, distance, minWallCount);
        }

        public static CastCondition WallCountInterval(int maxWalls, int minWalls)
        {
            return new CastCondition(maxWalls, null, minWalls);
        }
        
}
}
