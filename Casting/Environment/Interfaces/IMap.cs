

namespace Casting.Environment.Interfaces
{
    public interface IMap
    {
        int Width { get; }
        int Height { get; }

        bool IsInRange(int x, int y);

        int this[int x, int y] { get; }
    }
}
