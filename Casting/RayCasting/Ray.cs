using System.Collections.Generic;
using Casting.Environment.Interfaces;
using Casting.RayCasting.Interfaces;

namespace Casting.RayCasting
{
    public class Ray
    {
        public List<DistanceWrapper<ICrossable>> ObjectsCrossed { get; }


        public Ray()
        {
            ObjectsCrossed = new List<DistanceWrapper<ICrossable>>();
        }


        public void Add(DistanceWrapper<ICrossable> element)
        {
            int i = 0;
            while (i >= 0)
            {
                if (i == ObjectsCrossed.Count)
                {
                    ObjectsCrossed.Add(element);
                    i = -1;
                }
                else
                {
                    if (ObjectsCrossed[i].Distance > element.Distance)
                    {
                        ObjectsCrossed.Insert(i, element);
                        i = -1;
                    }
                    else
                    {
                        i++;
                    }
                }
                
            }
        }
    }
}
