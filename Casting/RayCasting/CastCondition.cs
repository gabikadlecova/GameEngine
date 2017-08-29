using Casting.RayCasting.Interfaces;

namespace Casting.RayCasting
{
    public class CastCondition : ICastCondition
    {
        //ToDo check

        public bool IsMet { get; set; }

        public int? MaxWalls { get; }

        public double? MaxDistance { get; }

        private int _wallCount;

        private double _currentDistance;

        public void WallCrossed(double distance)
        {
            _wallCount++;
            _currentDistance = distance;

            if (MaxDistance != null)
                IsMet = _currentDistance >= MaxDistance;

            if (MaxWalls != null)
            {
                IsMet = _wallCount >= MaxWalls;
            }
        }

        //ToDo distance sets off only if a wall had been crossed

        public void Reset()
        {
            IsMet = false;
            _wallCount = 0;
            _currentDistance = 0;
        }

        protected CastCondition(int? wallNumber, double? maxMaxDistance)
        {
            MaxWalls = wallNumber;
            MaxDistance = maxMaxDistance;
            IsMet = false;
            _wallCount = 0;
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
