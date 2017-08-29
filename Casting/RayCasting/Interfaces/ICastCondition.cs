namespace Casting.RayCasting.Interfaces
{
    public interface ICastCondition
    {
        bool IsMet { get; }
        void WallCrossed(double distance);

        void Reset();
    }
}
