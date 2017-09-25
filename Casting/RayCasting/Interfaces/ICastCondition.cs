namespace Casting.RayCasting.Interfaces
{
    /// <summary>
    /// Limits raycasting with a defined condition.
    /// </summary>
    public interface ICastCondition
    {
        /// <summary>
        /// Determines whether the condition has been met yet.
        /// </summary>
        bool IsMet { get; }
        /// <summary>
        /// Updates the condition's data of crossed objects
        /// </summary>
        /// <param name="distance">DIstance of the last obstacle</param>
        void ObstacleCrossed(double distance);
        
        /// <summary>
        /// Updates the current distance of the ray
        /// </summary>
        /// <param name="distance">Current distance of the ray end</param>
        void UpdateDistance(double distance);

        /// <summary>
        /// Resets all data of the class
        /// </summary>
        void Reset();
    }
}
