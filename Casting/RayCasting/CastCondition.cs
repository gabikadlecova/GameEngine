using Casting.RayCasting.Interfaces;

namespace Casting.RayCasting
{
    public class CastCondition : ICastCondition
    {
        //ToDo check

        public bool IsMet { get; set; }

        public int? MaxWalls { get; protected set; }

        public double? MaxDistance { get; protected set; }

        protected int _obstacleCount;

        protected double _currentDistance;

        public virtual void ObstacleCrossed(double distance)
        {
            _obstacleCount++;
            _currentDistance = distance;

            if (MaxDistance != null)
                IsMet = _currentDistance >= MaxDistance;

            if (MaxWalls != null)
            {
                IsMet = _obstacleCount >= MaxWalls;
            }
        }

        //ToDo distance sets off only if a wall had been crossed

        public void Reset()
        {
            IsMet = false;
            _obstacleCount = 0;
            _currentDistance = 0;
        }

        protected CastCondition(int? wallNumber, double? maxDistance)
        {
            MaxWalls = wallNumber;
            MaxDistance = maxDistance;
            IsMet = false;
            _obstacleCount = 0;
            _currentDistance = 0;
        }

        public static CastCondition LimitWalls(int wallNumber)
        {
            return new CastCondition(wallNumber, null);
        }

        public static CastCondition CastDistance(double distance, int minWallCount)
        {
            return new CastCondition(minWallCount, distance);
        }
        
}
}
