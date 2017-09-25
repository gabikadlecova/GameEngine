using System.Collections.Generic;
using Casting.Environment.Interfaces;
using Casting.RayCasting.Interfaces;

namespace Casting.RayCasting
{
    /// <summary>
    /// Thiss class contains data about the obstacles crossed by a specific ray
    /// </summary>
    public class Ray
    {
        /// <summary>
        /// List of object crossed with additional data
        /// </summary>
        public List<DistanceWrapper<ICrossable>> ObjectsCrossed { get; }

        /// <summary>
        /// Initializes a new ray
        /// </summary>
        public Ray()
        {
            ObjectsCrossed = new List<DistanceWrapper<ICrossable>>();
        }

        /// <summary>
        /// Adds another obstacle to the crossed object list so that the list remains sorted. 
        /// </summary>
        /// <param name="element">Obstacle to be added</param>
        public void Add(DistanceWrapper<ICrossable> element)
        {
            int i = 0;
            bool added = false;
            while (!added)
            {
                if (i == ObjectsCrossed.Count)
                {
                    ObjectsCrossed.Add(element);
                    added = true;
                }
                else
                {
                    if (ObjectsCrossed[i].Distance > element.Distance)
                    {
                        ObjectsCrossed.Insert(i, element);
                        added = true;
                    }
                    i++;
                    
                }
                
            }
        }
    }
}
