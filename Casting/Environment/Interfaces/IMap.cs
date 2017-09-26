namespace Casting.Environment.Interfaces
{
    /// <summary>
    /// Provides game map data
    /// </summary>
    public interface IMap
    {
        /// <summary>
        /// Map width
        /// </summary>
        int Width { get; }
        /// <summary>
        /// Map height
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Determines whether a specified point in space lies on the map 
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>Returns true if the specified point lies on the map</returns>
        bool IsInRange(int x, int y);

        /// <summary>
        /// Provides direct access to map tile value
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>Integer value of a specified map tile</returns>
        int this[int x, int y] { get; }
    }
}
