namespace Casting.RayCasting
{
    public class DistanceWrapper<TObj>
    {
        public double Distance { get; }

        /// <summary>
        /// Defines the x coordinate of the texture, or precisely the exact location of the ray hit
        /// </summary>
        public double TextureXRatio { get; }

        /// <summary>
        /// Indicates on which side did the ray hit
        /// </summary>
        public Side Side { get; }
        public TObj Element { get; set; }

        public DistanceWrapper(double distance, double textureXRatio, Side side, TObj element)
        {
            Distance = distance;
            Element = element;
            TextureXRatio = textureXRatio;
            Side = side;
        }

    }
}
