namespace Casting.RayCasting.Interfaces
{
    public interface IVector
    {
        double X { get; }

        double Y { get; }

        double Norm { get; }

        void Resize(double multiplier);
        
    }
}
